using PrayerForumsLibrary.Database;

namespace PrayerForumsLibrary.Interfaces
{
    public interface IInsertReply : IGetDetailsBase
    {
        Replies Response { get; set; }
    }
}