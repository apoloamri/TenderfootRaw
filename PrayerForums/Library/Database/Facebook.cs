using Tenderfoot.Database;

namespace PrayerForums.Library.Database
{
    public class Facebook : TfEntity
    {
        [NotNull]
        public string user_id { get; set; }

        [NotNull]
        public int? member_id { get; set; }
    }
}
