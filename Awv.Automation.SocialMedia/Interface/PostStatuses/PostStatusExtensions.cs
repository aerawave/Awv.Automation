namespace Awv.Automation.SocialMedia.Interface.PostStatuses
{
    public static class PostStatusExtensions
    {
        public static bool IsSuccess(this IPostStatus status) => status is IPostSuccess;
        public static bool IsFailure(this IPostStatus status) => status is IPostFailure;
        public static bool IsPartialSuccess(this IPostStatus status) => status is IPostPartialSuccess;
    }
}
