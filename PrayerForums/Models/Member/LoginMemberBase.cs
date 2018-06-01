using PrayerForums.Library;
using PrayerForums.Library.Database;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;

namespace PrayerForums.Models.Member
{
    public class LoginMemberBase : TfBaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Members Member { get; set; }

        internal ValidationResult ValidateUsernameSession(string sessionIdValue)
        {
            var members = _Schemas.Members;
            members.Entity.username = sessionIdValue;
            members.Entity.active = EnumActive.Active;
            if (members.Count == 0)
            {
                return TfValidationResult.Compose(
                    "SessionExpired",
                    nameof(this.Username),
                    nameof(this.Password));
            }
            return null;
        }

        internal ValidationResult ValidateUsernamePassword()
        {
            var members = _Schemas.Members;
            members.Entity.username = this.Username;
            members.Entity.password = this.Password;
            if (members.Count == 0)
            {
                return TfValidationResult.Compose(
                    "InvalidUsernamePassword",
                    nameof(this.Username),
                    nameof(this.Password));
            }
            return null;
        }

        internal void GetMember(string sessionIdValue)
        {
            var members = _Schemas.Members;
            members.Entity.username = sessionIdValue;
            members.Entity.active = EnumActive.Active;
            var member = members.Select.Entity;
            this.Member = member;
            this.Member.password = null;
        }
    }
}