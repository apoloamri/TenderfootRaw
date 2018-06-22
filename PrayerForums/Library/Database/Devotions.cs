using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace PrayerForums.Library.Database
{
    public class Devotions : TfEntity
    {
        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.All)]
        public string title { get; set; }

        [NotNull]
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.All)]
        public string message { get; set; }
    }
}