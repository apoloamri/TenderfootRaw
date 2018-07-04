namespace Tenderfoot.Database.Tables
{
    public class Emails : TfEntity
    {
        [NotNull]
        [Length(100)]
        public string mail_from { get; set; }

        [NotNull]
        [Length(100)]
        public string mail_to { get; set; }

        [NotNull]
        [Text]
        public string mail_title { get; set; }

        [NotNull]
        [Text]
        public string mail_body { get; set; }
    }
}
