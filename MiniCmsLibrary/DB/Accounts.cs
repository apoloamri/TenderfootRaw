using MiniCmsLibrary.Enums;
using Tenderfoot.Database;

namespace MiniCmsLibrary.DB
{
    public class Accounts : TfEntity
    {
        [Length(50)]
        [NotNull]
        public string username { get; set; }

        [Length(50)]
        [NotNull]
        public string password { get; set; }

        [Length(50)]
        [NotNull]
        public string email_address { get; set; }

        [Length(50)]
        public string last_name { get; set; }

        [Length(50)]
        public string first_name { get; set; }
        
        public EnumAdmin? admin { get; set; }
    }
}
