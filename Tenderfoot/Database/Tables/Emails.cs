namespace Tenderfoot.Database.Tables
{
    public class Emails : TfEntity
    {
        [NotNull]
        public string mail_from { get; set; }

        [NotNull]
        public string mail_to { get; set; }

        [NotNull]
        public string mail_title { get; set; }

        [NotNull]
        public string mail_body { get; set; }
    }
}
