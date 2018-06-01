using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace PrayerForums.Library.Database
{
    public class Requests : Entity
    {
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
        [ValidateInput(InputType.Email)]
        public string email { get; set; }

        [NotNull]
        [Text]
        [Input]
        [RequireInput(HttpMethod.POST)]
        [ValidateInput(InputType.All)]
        public string request { get; set; }

        [NotNull]
        [Input]
        [RequireInput(HttpMethod.POST)]
        [ValidateInput(InputType.Enum)]
        [Default("0")]
        public EnumPublic? response { get; set; }

        [NotNull]
        [Input]
        [RequireInput(HttpMethod.POST)]
        [ValidateInput(InputType.Enum)]
        [Default("0")]
        public EnumActive? send_email { get; set; }

        [NotNull]
        [Default("0")]
        public EnumPrayerType? prayer_type { get; set; }
    }
}