using PrayerForums.Library.Database;

namespace PrayerForums.Library.Function.Interface
{
    public interface IGetReport
    {
        int? PraiseId { get; set; }
        Praises Details { get; set; }
    }
}