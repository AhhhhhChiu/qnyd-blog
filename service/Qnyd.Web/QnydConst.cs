namespace Qnyd
{
    internal static class QnydConst
    {
        public const string Route = "/api/v1/";

        public const string RouteWithController = Route + "[controller]";

        public const string RouteWithControllerAction = RouteWithController+"/[action]";

    }
}
