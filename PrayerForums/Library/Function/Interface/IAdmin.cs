using PrayerForums.Library.Database;

namespace PrayerForums.Library.Function.Interface
{
    public interface IAdmin
    {
        string SessionIdValue { get; }
        Devotions DevotionalMessage { get; set; }
    }
}