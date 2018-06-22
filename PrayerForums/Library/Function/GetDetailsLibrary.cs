using PrayerForums.Library.Database;
using PrayerForums.Models.Prayer.Interface;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Library.Function
{
    public class GetDetailsLibrary : TfLibrary
    {
        public ValidationResult ValidateRequestId(IGetDetails getDetails)
        {
            var requests = _Schemas.Requests;
            requests.Entity.id = getDetails.RequestId;
            if (!this.IsAdmin(getDetails))
            {
                requests.Entity.publicity = EnumPublic.Public;
            }
            if (requests.Count == 0)
            {
                return TfValidationResult.Compose(
                    "DataNotFound",
                    nameof(getDetails.RequestId));
            }
            return null;
        }
        
        public void GetRequest(IGetDetails getDetails)
        {
            var requests = _Schemas.Requests;
            requests.Entity.id = getDetails.RequestId;
            getDetails.Requests = requests.Select.Entity;
        }

        public void GetReplies(IGetDetails getDetails)
        {
            var replies = _Schemas.Replies;
            replies.Entity.request_id = getDetails.RequestId;
            getDetails.Replies = replies.Select.Result;
            getDetails.ReplyCount = getDetails.Replies.Count;
        }

        private bool IsAdmin(IGetDetails getDetails)
        {
            if (!getDetails.SessionActive)
            {
                return false;
            }
            var member = _Schemas.Members;
            member.Entity.username = getDetails.SessionIdValue;
            member.Entity.admin = EnumAdmin.Admin;
            return member.Count == 1;
        }
    }
}