using MiniCmsLibrary.DB;
using Tenderfoot.Database;

namespace MiniCmsLibrary
{
    public class _DB : Schemas
    {
        public static Schema<Accounts> Accounts => CreateTable<Accounts>("admins");
    }
}
