using System;

namespace Tenderfoot.Database.Tables
{
    public class Accesses : TfEntity
    {
        [Length(100)]
        [NotNull]
        public string key { get; set; }

        [Length(100)]
        [NotNull]
        public string secret { get; set; }

        [NotNull]
        [Default("1")]
        public int? active { get; set; }

        [NonTableColumn]
        public override DateTime? insert_time { get; set; }
    }
}
