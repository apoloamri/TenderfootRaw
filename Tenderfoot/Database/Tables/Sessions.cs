using System;

namespace Tenderfoot.Database.Tables
{
    public class Sessions : Entity
    {
        [NotNull]
        [Encrypt]
        [Length(100)]
        public string session_id { get; set; }

        [NotNull]
        [Encrypt]
        [Length(100)]
        public string session_key { get; set; }

        [NotNull]
        public DateTime? session_time { get; set; }
    }
}
