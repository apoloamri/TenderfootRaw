using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForumsLibrary
{
    public class GetReportLibrary : TfLibrary
    {
        public ValidationResult ValidatePraiseId(IGetReport getReport)
        {
            var praises = _Schemas.Praises;
            praises.Entity.id = getReport.PraiseId;
            if (praises.Count == 0)
            {
                return TfValidationResult.Compose(
                    "DataNotFound",
                    nameof(getReport.PraiseId));
            }
            return null;
        }

        public void GetPraise(IGetReport getReport)
        {
            var praises = _Schemas.Praises;
            praises.Entity.id = getReport.PraiseId;
            getReport.Details = praises.Select.Entity;
        }
    }
}