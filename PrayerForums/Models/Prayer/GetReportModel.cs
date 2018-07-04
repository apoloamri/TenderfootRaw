using PrayerForumsLibrary;
using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Prayer
{
    public class GetReportModel : TfModel<GetReportLibrary>, IGetReport
    {
        [RequireInput]
        [Input(InputType.Numeric)]
        public int? PraiseId { get; set; }

        [Output]
        public Praises Details { get; set; }
        
        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValidRequireInputs())
            {
                yield return this.Library.ValidatePraiseId(this);
            }
        }
        
        public override void MapModel()
        {
            this.Library.GetPraise(this);
        }
    }
}