using PrayerForums.Library;
using PrayerForums.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.Tools.Extensions;

namespace PrayerForums.Models.Prayer
{
    public class GetRequestsModel : TfModel
    {
        [Input]
        [RequireInput]
        public int Page { get; set; } = 1;

        [Input]
        [RequireInput]
        public int Count { get; set; } = 10;

        [Output]
        public List<Dictionary<string, object>> Result { get; set; } = new List<Dictionary<string, object>>();

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void MapModel()
        {
            var member = _Schemas.Requests;
            member.Entity.response = EnumPublic.Public;
            member.Entity.prayer_type = EnumPrayerType.PrayerRequest;
            member.Case.Paginate(this.Page, this.Count);
            member.Case.OrderBy(member._(x => x.insert_time), Order.DESC);
            foreach (var item in member.Select.Entities)
            {
                var dictionary = item.ToDictionary();
                dictionary["request"] = item.request.Truncate(75);
                dictionary["insert_time"] = item.insert_time?.ToString("MMMM dd, yyyy");
                this.Result.Add(dictionary);
            }
        }
    }
}