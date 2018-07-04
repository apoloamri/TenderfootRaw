using PrayerForumsLibrary.Database;

namespace PrayerForumsLibrary.Interfaces
{
    public interface IGetReport
    {
        int? PraiseId { get; set; }
        Praises Details { get; set; }
    }
}