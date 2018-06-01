using PrayerForums.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Prayer
{
    public class GetDetailsModel : TfModel<GetDetailsBase>
    {
        [Input]
        [RequireInput]
        public int? RequestId { get; set; }

        [Output]
        public Requests Result { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValidRequireInputs())
            {
                yield return this.Library.ValidateRequestId();
            }
        }

        public override void MapModel()
        {
            this.Library.GetRequest();
        }
    }
}