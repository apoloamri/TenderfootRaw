using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace PrayerForums.Library.Database
{
    public class Praises : TfEntity
    {
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
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.All, 50)]
        public string title { get; set; }

        [NotNull]
        [Text]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.All)]
        public string message { get; set; }
        
        [Input(InputType.URL)]
        public string image_url { get; set; }
    }
}
