using PrayerForums.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Prayer
{
    public class GetReportModel : TfModel
    {
        [RequireInput]
        [Input(InputType.Numeric)]
        public int? PraiseId { get; set; }

        [Output]
        public Praises Result { get; set; }
        
        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValidRequireInputs())
            {
                yield return this.ValidatePraiseId();
            }
        }

        private ValidationResult ValidatePraiseId()
        {
            var praises = _Schemas.Praises;
            praises.Entity.id = this.PraiseId;
            if (praises.Count == 0)
            {
                return TfValidationResult.Compose(
                    "DataNotFound",
                    nameof(this.PraiseId));
            }
            return null;
        }

        public override void MapModel()
        {
            var praises = _Schemas.Praises;
            praises.Entity.id = this.PraiseId;
            this.Result = praises.Select.Entity;
        }
    }
}