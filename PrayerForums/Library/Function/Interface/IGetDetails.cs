using PrayerForums.Library.Database;
using System.Collections.Generic;

namespace PrayerForums.Models.Prayer.Interface
{
    public interface IGetDetails
    {
        int? RequestId { get; set; }
        Requests Requests { get; set; }
        List<dynamic> Replies { get; set; }
        int ReplyCount { get; set; }
        string SessionIdValue { get; }
        bool SessionActive { get; }
    }
}