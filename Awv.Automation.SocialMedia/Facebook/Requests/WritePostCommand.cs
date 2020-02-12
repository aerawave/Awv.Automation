using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using Awv.Automation.SocialMedia.Facebook.Posts.Statuses;
using Awv.Automation.SocialMedia.Interface.PostStatuses;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awv.Automation.SocialMedia.Facebook.Requests
{
    public class WritePostCommand
    {
        public const string FeedDirectory = "feed";
        public const string PhotosDirectory = "photos";

        public FacebookFileClient Client { get; set; }
        public IFacebookPost Post { get; set; }

        public WritePostCommand(FacebookFileClient client, IFacebookPost post)
        {
            Client = client;
            Post = post;
        }

        public async Task<IPostStatus> SendAsync()
        {
            if (Post is IPhotoPost) return await SendImageAsync();
            else if (Post is IMultiPhotoPost) return await SendImagesAsync();
            else return await SendCaptionOnlyAsync();
        }

        private async Task<IPostStatus> SendCaptionOnlyAsync()
        {
            var caption = Post.GetCaption();

            var guid = Guid.NewGuid();
            var json = new JObject();
            var jsonKey = $"{guid}.json";
            var jsonPath = Path.Combine(Client.TargetDirectory, PhotosDirectory, jsonKey);
            var directoryPath = Path.GetDirectoryName(jsonPath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using var jsonFile = File.Open(jsonPath, FileMode.Create);
            using var jsonWriter = new StreamWriter(jsonFile);


            json["id"] = guid;
            json["target"] = Client.Target;
            json["caption"] = caption;

            jsonWriter.Write(json.ToString());

            return await Task.FromResult(new PostSuccess(Post, new PostData(guid.ToString(), $"{Client.Target}_{guid}")));

        }

        private async Task<IPostStatus> SendImageAsync()
        {
            var post = Post as IPhotoPost;
            var caption = post.GetCaption();
            var imageKey = post.GetImageKey();
            var image = post.GetImage();

            var guid = Guid.NewGuid();
            var json = new JObject();
            var jsonKey = $"{guid}.json";
            var jsonPath = Path.Combine(Client.TargetDirectory, PhotosDirectory, jsonKey);
            var imagePath = Path.Combine(Client.TargetDirectory, PhotosDirectory, imageKey);
            var directoryPath = Path.GetDirectoryName(jsonPath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using var imageFile = File.Open(imagePath, FileMode.Create);
            using var jsonFile = File.Open(jsonPath, FileMode.Create);
            using var jsonWriter = new StreamWriter(jsonFile);


            json["id"] = guid;
            json["target"] = Client.Target;
            json["caption"] = caption;
            json["image-key"] = imageKey;

            image.SaveAsJpeg(imageFile);
            jsonWriter.Write(json.ToString());

            return await Task.FromResult(new PostSuccess(Post, new PostData(guid.ToString(), $"{Client.Target}_{guid}")));

        }

        private async Task<IPostStatus> SendImagesAsync()
        {
            var post = Post as IMultiPhotoPost;
            var caption = post.GetCaption();
            var children = post.GetChildPosts();

            var guid = Guid.NewGuid();
            var json = new JObject();
            var jsonImageArray = new JArray();
            var jsonKey = $"{guid}.json";
            var jsonPath = Path.Combine(Client.TargetDirectory, FeedDirectory, jsonKey);
            var directoryPath = Path.GetDirectoryName(jsonPath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using var jsonFile = File.Open(jsonPath, FileMode.Create);
            using var jsonWriter = new StreamWriter(jsonFile);

            var statusTasks = children.Select(child => Client.SendAsync(child)).ToArray();

            await Task.WhenAll(statusTasks);
            var statuses = new List<PostStatus>();

            foreach (var task in statusTasks)
                statuses.Add(await task as PostStatus);

            json["id"] = guid;
            json["target"] = Client.Target;
            json["caption"] = caption;
            json["child-posts"] = jsonImageArray;

            foreach(var status in statuses)
            {
                if (status.Data.HasValue)
                {
                    jsonImageArray.Add(status.Data.Value.Id);
                }
            }

            jsonWriter.Write(json.ToString());

            return await Task.FromResult(new PostSuccess(Post, new PostData($"{Client.Target}_{guid}", null)));
        }
    }
}
