using PrayerForums.Library.Database;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;

namespace PrayerForums.Models.Prayer
{
    public class GetDetailsBase : TfBaseModel
    {
        public int? RequestId { get; set; }
        public Requests Result { get; set; }

        internal ValidationResult ValidateRequestId()
        {
            var requests = _Schemas.Requests;
            requests.Entity.id = this.RequestId;
            if (requests.Count == 0)
            {
                return TfValidationResult.Compose(
                    "DataNotFound",
                    nameof(this.RequestId));
            }
            return null;
        }

        internal void GetRequest()
        {
            var requests = _Schemas.Requests;
            requests.Entity.id = this.RequestId;
            this.Result = requests.Select.Entity;
        }
    }
}