using PrayerForumsLibrary;
using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Admin
{
    public class DeletePraisesModel : TfModel, IGetReport
    {
        [Input(InputType.Numeric)]
        [RequireInput]
        public int? PraiseId { get; set; }

        public Praises Details { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return new GetReportLibrary().ValidatePraiseId(this);
        }

        public override void HandleModel()
        {
            var praises = _Schemas.Praises;
            praises.Entity.id = this.PraiseId;
            praises.Delete();
        }
    }
}