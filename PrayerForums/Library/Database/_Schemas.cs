using Tenderfoot.Database;

namespace PrayerForums.Library.Database
{
    public class _Schemas : Schemas
    {
        public static Schema<Members> Members => CreateTable<Members>("members");
        public static Schema<Requests> Requests => CreateTable<Requests>("requests");
    }
}
