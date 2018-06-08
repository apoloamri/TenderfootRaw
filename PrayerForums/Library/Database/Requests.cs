using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace PrayerForums.Library.Database
{
    public class Requests : TfEntity
    {
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
        [Input(InputType.Email)]
        public string email { get; set; }

        [NotNull]
        [Text]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.All)]
        public string request { get; set; }

        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.Enum)]
        [Default("0")]
        public EnumPublic? publicity { get; set; }

        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.Enum)]
        [Default("0")]
        public EnumActive? send_email { get; set; }
    }
}