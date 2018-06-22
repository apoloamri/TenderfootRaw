using PrayerForums.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.Tools.Extensions;

namespace PrayerForums.Models.Prayer
{
    public class GetPraisesModel : TfModel
    {
        [Input]
        [RequireInput]
        public int Page { get; set; } = 1;

        [Input]
        [RequireInput]
        public int Count { get; set; } = 10;

        [Output]
        public List<Dictionary<string, object>> Result { get; set; } = new List<Dictionary<string, object>>();

        [Output]
        public int TotalPages { get; set; } = 0;

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void MapModel()
        {
            var praises = _Schemas.Praises;
            praises.Case.Paginate(this.Page, this.Count);
            praises.Case.OrderBy(praises.Column(x => x.insert_time), OrderBy.DESC);
            var result = praises.Select.Entities;
            foreach (var item in result)
            {
                var dictionary = item.ToDictionary();
                dictionary["message"] = item.message.Truncate(150);
                dictionary["insert_time"] = item.insert_time?.ToString("MMMM dd, yyyy");
                this.Result.Add(dictionary);
            }
            this.TotalPages = praises.PageCount(this.Count);
        }
    }
}