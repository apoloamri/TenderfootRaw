using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace PrayerForums.Library.Database
{
    public class Replies : TfEntity
    {
        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.Numeric)]
        public int? request_id { get; set; }

        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.String, 50)]
        public string lastname { get; set; }

        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.String, 50)]
        public string firstname { get; set; }

        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.Email, 50)]
        public string email { get; set; }

        [NotNull]
        [Text]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.All)]
        public string response { get; set; }
    }
}
