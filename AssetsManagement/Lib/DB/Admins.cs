using Tenderfoot.Database;

namespace AssetsManagement.Lib.DB
{
    public class Admins : TfEntity
    {
        [NotNull]
        public string username { get; set; }

        [NotNull]
        public string password { get; set; }

        [NotNull]
        public string[] page_access { get; set; }

        [NotNull]
        [Default("0")]
        public bool? accounting { get; set; }
    }
}