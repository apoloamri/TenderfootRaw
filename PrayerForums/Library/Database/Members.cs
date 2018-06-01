﻿using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace PrayerForums.Library.Database
{
    public class Members : Entity
    {
        [NotNull]
        [Input]
        [RequireInput(HttpMethod.POST)]
        [ValidateInput(InputType.Email)]
        public string email { get; set; }

        [NotNull]
        [Input]
        [RequireInput(HttpMethod.POST)]
        [ValidateInput(InputType.String)]
        public string lastname { get; set; }

        [NotNull]
        [Input]
        [RequireInput(HttpMethod.POST)]
        [ValidateInput(InputType.String)]
        public string firstname { get; set; }

        [NotNull]
        [Input]
        [RequireInput(HttpMethod.POST)]
        [ValidateInput(InputType.String)]
        public string username { get; set; }

        [NotNull]
        [Input]
        [RequireInput(HttpMethod.POST)]
        [ValidateInput(InputType.String)]
        public string password { get; set; }

        [Length(100)]
        public string activation_key { get; set; }

        [NotNull]
        [Default("0")]
        public EnumActive? active { get; set; }

        [NotNull]
        [Default("0")]
        public EnumAdmin? admin { get; set; }
    }
}