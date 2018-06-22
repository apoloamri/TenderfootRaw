using PrayerForums.Library.Database;

namespace PrayerForums.Library.Function.Interface
{
    public interface ILoginMember
    {
        string Username { get; set; }
        string Password { get; set; }
        Members Member { get; set; }
        string SessionIdValue { get; }
    }
}