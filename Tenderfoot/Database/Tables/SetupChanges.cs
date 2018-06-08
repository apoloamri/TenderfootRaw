namespace Tenderfoot.Database.Tables
{
    public class SetupChanges : TfEntity
    {
        [NotNull]
        [Default("0")]
        public bool Encrypted { get; set; }
    }
}
