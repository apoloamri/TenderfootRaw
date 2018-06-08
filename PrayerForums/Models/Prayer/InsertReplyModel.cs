using PrayerForums.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Prayer
{
    public class InsertReplyModel : TfModel<InsertReplyBase>
    {
        [Input]
        [Output]
        public Replies Response { get; set; } = new Replies();

        public GetDetailsBase DetailsBase { get; set; }

        public override void BeforeStartUp()
        {
            this.DetailsBase = new GetDetailsBase() { RequestId = this.Response.request_id };
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValidRequireInputs())
            {
                yield return this.DetailsBase.ValidateRequestId();
            }
        }

        public override void HandleModel()
        {
            this.Library.InsertReply();
        }
    }
}