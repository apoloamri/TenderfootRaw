using PrayerForumsLibrary.Database;
using System.Collections.Generic;

namespace PrayerForumsLibrary.Interfaces
{
    public interface IGetDetails : IGetDetailsBase
    {
        List<dynamic> Replies { get; set; }
        int ReplyCount { get; set; }
        string SessionIdValue { get; }
        bool SessionActive { get; }
    }

    public interface IGetDetailsBase
    {
        int? RequestId { get; set; }
        Requests Requests { get; set; }
    }
}