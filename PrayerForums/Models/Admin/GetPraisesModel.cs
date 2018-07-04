using PrayerForumsLibrary.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Admin
{
    public class GetPraisesModel : TfModel
    {
        [Input(InputType.Numeric)]
        public int? Page { get; set; } = 1;

        [Input(InputType.Numeric)]
        public int? Count { get; set; } = 10;

        [Output]
        public List<Praises> Praises { get; set; }

        [Output]
        public long TotalPages { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void MapModel()
        {
            var praises = _Schemas.Praises;
            praises.Case.Paginate(this.Page, this.Count);
            praises.Case.OrderBy(praises.Column(x => x.insert_time), OrderBy.DESC);
            this.Praises = praises.Select.Entities;
            this.TotalPages = praises.PageCount(this.Count);
        }
    }
}