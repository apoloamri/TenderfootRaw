using PrayerForums.Library.Database;
using PrayerForums.Library.Function;
using PrayerForums.Models.Prayer.Interface;
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

        private readonly GetDetailsLibrary getDetailsLibrary;
        private readonly IGetDetails getDetails;
        
        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValidRequireInputs())
            {
                getDetails.RequestId = this.Response.request_id;
                yield return getDetailsLibrary.ValidateRequestId(getDetails);
            }
        }

        public override void HandleModel()
        {
            this.Library.InsertReply(this);
        }
    }
}