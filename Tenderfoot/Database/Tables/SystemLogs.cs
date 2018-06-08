namespace Tenderfoot.Database.Tables
{
    public class SystemLogs : TfEntity
    {
        [Text]
        [NotNull]
        public string message { get; set; }
    }
}
