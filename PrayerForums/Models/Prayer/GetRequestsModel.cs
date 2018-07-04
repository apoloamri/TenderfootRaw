using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Enums;
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

        [Output]
        public int TotalPages { get; set; } = 0;

        [Output]
        public int TotalRequests { get; set; } = 0;

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void MapModel()
        {
            var requests = _Schemas.Requests;
            if (!this.IsAdmin())
            {
                requests.Entity.publicity = EnumPublic.Public;
            }
            requests.Case.Paginate(this.Page, this.Count);
            requests.Case.OrderBy(requests.Column(x => x.insert_time), OrderBy.DESC);
            var result = requests.Select.Entities;
            foreach (var item in result)
            {
                var replies = _Schemas.Replies;
                replies.Entity.request_id = item.id;
                var replyCount = replies.Count;
                var dictionary = item.ToDictionary();
                dictionary["has_replies"] = replyCount > 0;
                dictionary["request"] = item.request.Truncate(75);
                dictionary["insert_time"] = item.insert_time?.ToString("MMMM dd, yyyy");
                this.Result.Add(dictionary);
            }
            this.TotalPages = requests.PageCount(this.Count);
            this.TotalRequests = (int)requests.Count;
        }

        private bool IsAdmin()
        {
            if (!this.SessionActive)
            {
                return false;
            }
            var member = _Schemas.Members;
            member.Entity.username = this.SessionIdValue;
            member.Entity.admin = EnumAdmin.Admin;
            return member.Count == 1;
        }
    }
}