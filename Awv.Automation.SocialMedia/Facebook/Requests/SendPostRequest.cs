using Awv.Automation.SocialMedia.Facebook.Posts;
using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using Awv.Automation.SocialMedia.Facebook.Posts.Statuses;
using Awv.Automation.SocialMedia.Interface.PostStatuses;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Awv.Automation.SocialMedia.Facebook.Requests
{
    public class SendPostRequest
    {
        public const string FeedEndpoint = "feed";
        public const string PhotosEndpoint = "photos";
        public FacebookClient Client { get; set; }
        public IFacebookPost Post { get; set; }

        public SendPostRequest(FacebookClient client, IFacebookPost post)
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
            var created = DateTime.UtcNow;
            var boundary = created.Ticks.ToString("x");

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent(boundary))
            {
                content.Add(new StringContent(Client.AccessToken), "access_token");
                content.Add(new StringContent(caption), "message");

                try
                {
                    var response = await client.PostAsync(Client.GetUri(Client.GetTarget(), PhotosEndpoint), content);
                    var json = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                        return new PostSuccess(Post, JsonConvert.DeserializeObject<PostData>(json));
                    else
                        return new PostFailure(Post, await response.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    return new PostFailure(Post, ex);
                }
            }
        }

        private async Task<IPostStatus> SendImageAsync()
        {
            var post = Post as IPhotoPost;
            var caption = Post.GetCaption();
            var imageKey = post.GetImageKey();
            var image = post.GetImage();
            var created = DateTime.UtcNow;
            var boundary = created.Ticks.ToString("x");

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent(boundary))
            using (var imageStream = new MemoryStream())
            {
                image.SaveAsJpeg(imageStream);
                imageStream.Seek(0, SeekOrigin.Begin);
                content.Add(new StringContent(Client.AccessToken), "access_token");
                content.Add(new StreamContent(imageStream), "filename", Path.GetFileName(imageKey));

                if (!string.IsNullOrWhiteSpace(caption))
                    content.Add(new StringContent(caption), "message");

                if (!post.IsPublished())
                    content.Add(new StringContent("false"), "published");

                try
                {
                    var response = await client.PostAsync(Client.GetUri(Client.GetTarget(), PhotosEndpoint), content);
                    var json = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                        return new PhotoPosted(post, JsonConvert.DeserializeObject<PostData>(json));
                    else
                        return new PostFailure(post, await response.Content.ReadAsStringAsync());
                }catch(Exception ex)
                {
                    return new PostFailure(post, ex);
                }
            }
        }

        private async Task<IPostStatus> SendImagesAsync()
        {
            var post = Post as IMultiPhotoPost;
            var caption = post.GetCaption();
            var children = post.GetChildPosts();
            var created = DateTime.UtcNow;
            var boundary = created.Ticks.ToString("x");

            var statusTasks = children.Select(child => Client.SendAsync(child)).ToArray();

            await Task.WhenAll(statusTasks);
            var statuses = new List<PostStatus>();

            foreach (var task in statusTasks)
                statuses.Add(await task as PostStatus);

            var makePost = true;
            var partial = false;
            if (statuses.All(status => status is IPostSuccess))
            {
                // all success
            } else if (statuses.All(status => status is IPostFailure))
            {
                // all failure
                makePost = false;
                return new PostFailure(Post, "All posts failed to post.");
            } else
            {
                makePost = post.GetPartialSuccessFunction()(statuses);
                partial = true;
                // partial success
            }

            if (makePost)
            {
                using (var client = new HttpClient())
                using (var content = new MultipartFormDataContent(boundary))
                {
                    content.Add(new StringContent(Client.AccessToken), "access_token");
                    content.Add(new StringContent(caption), "message");

                    var mediaIndex = 0;
                    foreach (var status in statuses)
                    {
                        if (status.Data.HasValue)
                        {
                            var media = new AttachedMedia(status.Data.Value);
                            var mediaContent = JsonConvert.SerializeObject(media);
                            content.Add(new StringContent(mediaContent), $"attached_media[{mediaIndex++}]");
                        }
                    }

                    try
                    {
                        var response = await client.PostAsync(Client.GetUri(Client.GetTarget(), FeedEndpoint), content);
                        var json = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                            return new PostSuccess(Post, JsonConvert.DeserializeObject<PostData>(json));
                        else
                            return new PostFailure(Post, await response.Content.ReadAsStringAsync());
                    }
                    catch (Exception ex)
                    {
                        return new PostFailure(Post, ex);
                    }
                }
            } else
            {
                if (partial)
                {
                    return new PostPartialSuccess(Post, null);
                } else
                {
                    return new PostFailure(Post, "Post failed.");
                }
            }

        }
    }
}
