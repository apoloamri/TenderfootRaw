namespace Tenderfoot.Mvc
{
    public interface ITfModel
    {
        string SessionKey { get; set; }
        string SessionId { get; set; }
        string SessionIdValue { get; }
        void NewSession(string sessionId);
        void GetSessionCookies();
        void SetSessionCookies();
    }
}
