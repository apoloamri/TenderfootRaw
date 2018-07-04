using Tenderfoot.Database;

namespace PrayerForumsLibrary.Database
{
    public class Facebook : TfEntity
    {
        [NotNull]
        public string user_id { get; set; }

        [NotNull]
        public int? member_id { get; set; }
    }
}
