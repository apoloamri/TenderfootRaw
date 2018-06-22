using PrayerForums.Library.Database;
using PrayerForums.Library.Function;
using PrayerForums.Models.Prayer;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Admin
{
    public class DeleteRequestsModel : TfModel
    {
        [Input(InputType.Numeric)]
        [RequireInput]
        public int? RequestId { get; set; }
        
        public override IEnumerable<ValidationResult> Validate()
        {
            var getDetailsModel = new GetDetailsModel
            {
                RequestId = this.RequestId
            };
            yield return new GetDetailsLibrary().ValidateRequestId(getDetailsModel);
        }

        public override void HandleModel()
        {
            var requests = _Schemas.Requests;
            requests.Entity.id = this.RequestId;
            requests.Delete();
        }
    }
}