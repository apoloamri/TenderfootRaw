using System;

namespace Tenderfoot.Database.Tables
{
    public class Sessions : TfEntity
    {
        [Length(100)]
        [Encrypt]
        [NotNull]
        public string session_id { get; set; }

        [Length(100)]
        [Encrypt]
        [NotNull]
        public string session_key { get; set; }

        [NotNull]
        public DateTime? session_time { get; set; }
    }
}
