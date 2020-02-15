using Awv.Automation.SocialMedia.Interface.PostStatuses;
using Awv.Automation.SocialMedia.Twitter.Tweets;
using Awv.Automation.SocialMedia.Twitter.Tweets.Statuses;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TweetSharp;

namespace Awv.Automation.SocialMedia.Twitter.Requests
{
    public class SendTweetRequest
    {
        public TwitterClient Client { get; set; }
        public ITweet Tweet { get; set; }

        public SendTweetRequest(TwitterClient client, ITweet tweet)
        {
            Client = client;
            Tweet = tweet;
        }

        public async Task<IPostStatus> SendAsync()
        {
            var images = Tweet.GetImages()?.ToArray();
            var caption = Tweet.GetCaption();

            var sendWithoutMedia = images.Length == 0;

            TwitterStatus createdStatus = null;



            var tweet = new SendTweetOptions();
            var sendTweet = true;
            var partial = false;

            tweet.Status = caption;

            if (!sendWithoutMedia && images.Length > 0)
            {
                var uploads = new List<UploadMediaOptions>();

                foreach (var kv in images)
                {
                    var name = kv.Key;
                    var image = kv.Value;
                    var stream = new MemoryStream();
                    image.SaveAsPng(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    uploads.Add(new UploadMediaOptions { Media = new MediaFile { FileName = kv.Key, Content = stream } });
                }

                var uploadTasks = uploads.Select(upload => Client.Service.UploadMediaAsync(upload)).ToArray();
                var responses = new List<TwitterAsyncResult<TwitterUploadedMedia>>();
                await Task.WhenAll(uploadTasks);
                foreach (var task in uploadTasks)
                    responses.Add(await task);

                var media = responses.Where(response => (int)response.Response.StatusCode >= 200 && (int)response.Response.StatusCode < 300).ToArray();

                tweet.MediaIds = media.Select(image => image.Value.Media_Id).ToArray();

                if (media.Length == responses.Count)
                {
                    // all success
                }
                else if (media.Length == 0)
                {
                    // all failure
                    sendTweet = false;
                    return new TweetFailure(Tweet, "All photos failed to post.");
                }
                else
                {
                    // partial success
                    sendTweet = Tweet.GetPartialSuccessFunction()(responses.Select(response =>
                    {
                        IPostStatus status;
                        if ((int)response.Response.StatusCode >= 200 && (int)response.Response.StatusCode < 300)
                            status = new MediaPosted(response.Value);
                        else status = new UploadFailed();
                        return status;
                    }).ToArray());
                    partial = true;
                }
            }

            if (sendTweet)
            {
                createdStatus = Client.Service.SendTweet(tweet);
                if (createdStatus != null)
                {
                    if (partial)
                        return new TweetPartialSuccess(Tweet, createdStatus) { Posted = true };
                    else
                        return new TweetSuccess(Tweet, createdStatus);
                }
                else
                {
                    return new TweetFailure(Tweet, "Tweet failed to send.");
                }
            }
            else
            {
                if (partial)
                {
                    return new TweetPartialSuccess(Tweet);
                }
            }
            return new TweetFailure(Tweet, "Tweet failed to send.");
        }
    }
}
