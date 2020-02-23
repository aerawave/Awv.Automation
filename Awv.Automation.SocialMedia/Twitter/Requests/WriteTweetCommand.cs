using Awv.Automation.SocialMedia.Interface.PostStatuses;
using Awv.Automation.SocialMedia.Twitter.Tweets;
using Awv.Automation.SocialMedia.Twitter.Tweets.Statuses;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TweetSharp;

namespace Awv.Automation.SocialMedia.Twitter.Requests
{
    public class WriteTweetCommand
    {
        public const string TweetsDirectory = "tweets";
        public const string PhotosDirectory = "photos";

        public TwitterFileClient Client { get; set; }
        public ITweet Tweet { get; set; }

        public WriteTweetCommand(TwitterFileClient client, ITweet tweet)
        {
            Client = client;
            Tweet = tweet;
        }

        public async Task<IPostStatus> SendAsync()
        {
            var guid = Guid.NewGuid();

            var jsonKey = $"{guid}.json";
            var jsonDirectoryPath = Path.Combine(Client.Output, TweetsDirectory);
            var photosDirectoryPath = Path.Combine(Client.Output, PhotosDirectory);
            var jsonFilePath = Path.Combine(jsonDirectoryPath, $"{guid}.json");

            if (!Directory.Exists(jsonDirectoryPath)) Directory.CreateDirectory(jsonDirectoryPath);

            using var jsonFile = File.Open(jsonFilePath, FileMode.Create);
            using var jsonWriter = new StreamWriter(jsonFile);

            var images = Tweet.GetImages()?.ToArray();
            var caption = Tweet.GetCaption();

            var sendWithoutMedia = images == null;


            var json = new JObject();
            var imagesArray = new JArray();

            json["id"] = guid;
            json["caption"] = caption;
            if (images?.Length > 0)
            {
                if (!Directory.Exists(photosDirectoryPath)) Directory.CreateDirectory(photosDirectoryPath);
                json["images"] = imagesArray;

                for (var i = 0; i < images.Length; i++)
                {
                    var imageGui = Guid.NewGuid();
                    var imageKey = $"{imageGui}.png";
                    var imagePath = Path.Combine(photosDirectoryPath, imageKey);
                    imagesArray.Add(imageKey);
                    using var imageFile = File.Open(imagePath, FileMode.Create);
                    images[i].Value.SaveAsPng(imageFile);
                }
            }

            jsonWriter.Write(json.ToString());
            var createdStatus = new TwitterStatus
            {
                IdStr = jsonKey
            };

            return new TweetSuccess(Tweet, createdStatus);
        }
    }
}
