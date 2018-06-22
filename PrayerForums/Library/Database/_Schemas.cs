using Tenderfoot.Database;

namespace PrayerForums.Library.Database
{
    public class _Schemas : Schemas
    {
        public static Schema<Devotions> Devotions => CreateTable<Devotions>("devotions");
        public static Schema<Facebook> Facebook => CreateTable<Facebook>("facebook");
        public static Schema<Members> Members => CreateTable<Members>("members");
        public static Schema<Praises> Praises => CreateTable<Praises>("praises");
        public static Schema<Requests> Requests => CreateTable<Requests>("requests");
        public static Schema<Replies> Replies => CreateTable<Replies>("replies");
    }
}
