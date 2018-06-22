using PrayerForums.Library.Database;
using PrayerForums.Library.Function;
using PrayerForums.Models.Prayer;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Admin
{
    public class DeletePraisesModel : TfModel
    {
        [Input(InputType.Numeric)]
        [RequireInput]
        public int? PraiseId { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            var getReportModel = new GetReportModel
            {
                PraiseId = this.PraiseId
            };
            yield return new GetReportLibrary().ValidatePraiseId(getReportModel);
        }

        public override void HandleModel()
        {
            var praises = _Schemas.Praises;
            praises.Entity.id = this.PraiseId;
            praises.Delete();
        }
    }
}