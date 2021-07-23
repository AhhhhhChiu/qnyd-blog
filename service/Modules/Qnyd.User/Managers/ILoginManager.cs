namespace Qnyd.User.Managers
{
    public interface ILoginManager
    {
        bool HasToken(string userName);
        bool IsTokenEqual(string userName, string comparerToken);
        string SetAccessToken(string userName);
        void SetAccessToken(string userName, string token);
    }
}