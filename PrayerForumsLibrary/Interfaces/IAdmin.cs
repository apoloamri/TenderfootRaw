using PrayerForumsLibrary.Database;

namespace PrayerForumsLibrary.Interfaces
{
    public interface IAdmin
    {
        string SessionIdValue { get; }
        Devotions DevotionalMessage { get; set; }
    }
}