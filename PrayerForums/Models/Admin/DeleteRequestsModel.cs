using PrayerForumsLibrary;
using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Admin
{
    public class DeleteRequestsModel : TfModel, IGetDetailsBase
    {
        [Input(InputType.Numeric)]
        [RequireInput]
        public int? RequestId { get; set; }

        public Requests Requests { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return new GetDetailsLibrary().ValidateRequestId(this);
        }

        public override void HandleModel()
        {
            var requests = _Schemas.Requests;
            requests.Entity.id = this.RequestId;
            requests.Delete();
        }
    }
}