using PrayerForums.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Admin
{
    public class GetRequestsModel : TfModel
    {
        [Input(InputType.Numeric)]
        public int? Page { get; set; } = 1;

        [Input(InputType.Numeric)]
        public int? Count { get; set; } = 10;

        [Input(InputType.Boolean)]
        public bool? Unanswered { get; set; } = true;

        [Output]
        public List<Requests> Requests { get; set; }

        [Output]
        public long TotalPages { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void MapModel()
        {
            var requests = _Schemas.Requests;
            if (this.Unanswered == true)
            {
                var replies = _Schemas.Replies;
                var relation = requests.Relation(requests.Column(x => x.id), replies.Column(x => x.request_id));
                requests.Case.Exists(replies, relation);
            }
            requests.Case.Paginate(this.Page, this.Count);
            requests.Case.OrderBy(requests.Column(x => x.insert_time), OrderBy.DESC);
            this.Requests = requests.Select.Entities;
            this.TotalPages = requests.PageCount((int)requests.Count);
        }
    }
}