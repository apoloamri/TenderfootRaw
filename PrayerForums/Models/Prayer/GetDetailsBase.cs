using PrayerForums.Library;
using PrayerForums.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;

namespace PrayerForums.Models.Prayer
{
    public class GetDetailsBase : TfBaseModel
    {
        public int? RequestId { get; set; }
        public Requests Result { get; set; }
        public List<dynamic> Replies { get; set; }
        public int ReplyCount { get; set; }
        public string SessionIdValue { get; set; }
        public bool SessionActive { get; set; }

        public ValidationResult ValidateRequestId()
        {
            var requests = _Schemas.Requests;
            requests.Entity.id = this.RequestId;
            if (!this.IsAdmin())
            {
                requests.Entity.publicity = EnumPublic.Public;
            }
            if (requests.Count == 0)
            {
                return TfValidationResult.Compose(
                    "DataNotFound",
                    nameof(this.RequestId));
            }
            return null;
        }
        
        public void GetRequest()
        {
            var requests = _Schemas.Requests;
            requests.Entity.id = this.RequestId;
            this.Result = requests.Select.Entity;
        }

        public void GetReplies()
        {
            var replies = _Schemas.Replies;
            replies.Entity.request_id = this.RequestId;
            this.Replies = replies.Select.Result;
            this.ReplyCount = this.Replies.Count;
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