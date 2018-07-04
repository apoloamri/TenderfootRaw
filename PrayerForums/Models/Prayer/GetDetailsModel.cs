using PrayerForumsLibrary;
using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Prayer
{
    public class GetDetailsModel : 
        TfModel<GetDetailsLibrary>,
        IGetDetails
    {
        [Input]
        [RequireInput]
        public int? RequestId { get; set; }

        [Output]
        public Requests Requests { get; set; }

        [Output]
        public List<dynamic> Replies { get; set; }

        [Output]
        public int ReplyCount { get; set; } = 0;

        public override void BeforeStartUp()
        {
            this.GetSessionCookies();
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValidRequireInputs())
            {
                yield return this.Library.ValidateRequestId(this);
            }
        }

        public override void MapModel()
        {
            this.Library.GetRequest(this);
            this.Library.GetReplies(this);
        }
    }
}