using PrayerForumsLibrary.Enums;
using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace PrayerForumsLibrary.Database
{
    public class Members : TfEntity
    {
        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.Email)]
        public string email { get; set; }

        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.String)]
        public string lastname { get; set; }

        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.String)]
        public string firstname { get; set; }

        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.String)]
        public string username { get; set; }

        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.String)]
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