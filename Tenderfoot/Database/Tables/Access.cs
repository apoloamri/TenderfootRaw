using System;

namespace Tenderfoot.Database.Tables
{
    public class Accesses : TfEntity
    {
        [NotNull]
        [Length(100)]
        public string key { get; set; }

        [NotNull]
        [Length(100)]
        public string secret { get; set; }

        [NotNull]
        [Default("1")]
        public int? active { get; set; }

        [NonTableColumn]
        public override DateTime? insert_time { get; set; }
    }
}
