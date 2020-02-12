namespace Awv.Automation.SocialMedia.Interface.Posts
{
    public interface IPostWithCaption<TClientType, TSendReturnType> : IPost<TClientType, TSendReturnType> where TClientType : ISocialMediaClient
    {
        string GetCaption();
    }
}
