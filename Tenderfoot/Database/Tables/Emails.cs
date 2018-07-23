namespace Tenderfoot.Database.Tables
{
    public class Emails : TfEntity
    {
        [Length(100)]
        [NotNull]
        public string mail_from { get; set; }

        [Length(100)]
        [NotNull]
        public string mail_to { get; set; }

        [NotNull]
        [Text]
        public string mail_title { get; set; }

        [NotNull]
        [Text]
        public string mail_body { get; set; }
    }
}
