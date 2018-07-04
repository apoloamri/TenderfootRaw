using PrayerForumsLibrary.Database;

namespace PrayerForumsLibrary.Interfaces
{
    public interface ILoginMember
    {
        string Username { get; set; }
        string Password { get; set; }
        Members Member { get; set; }
        string SessionIdValue { get; }
    }
}