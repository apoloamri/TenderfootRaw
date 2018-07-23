using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenderfoot.Database;

namespace AssetsManagement.Lib.DB
{
    public class _DB : Schemas
    {
        public static Schema<Admins> Admins => CreateTable<Admins>("admins");
    }
}
