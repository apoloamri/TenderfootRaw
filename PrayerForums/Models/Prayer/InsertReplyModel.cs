using PrayerForumsLibrary;
using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Prayer
{
    public class InsertReplyModel : 
        TfModel<InsertReplyLibrary>,
        IInsertReply
    {
        [Input]
        [Output]
        public Replies Response { get; set; } = new Replies();

        [RequireInput]
        public int? RequestId { get; set; }

        public Requests Requests { get; set; }

        public override void BeforeStartUp()
        {
            this.RequestId = this.Response.request_id;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValidRequireInputs())
            {
                yield return new GetDetailsLibrary().ValidateRequestId(this);
            }
        }

        public override void HandleModel()
        {
            this.Library.InsertReply(this);
        }
    }
}