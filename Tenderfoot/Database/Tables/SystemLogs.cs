namespace Tenderfoot.Database.Tables
{
    public class SystemLogs : Entity
    {
        [Text]
        [NotNull]
        public string message { get; set; }
    }
}
