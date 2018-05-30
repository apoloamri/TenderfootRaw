using PrayerForums.Library;
using PrayerForums.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Member
{
    public class LoginMemberModel : TfModel
    {
        [Input]
        [RequireInput(HttpMethod.POST)]
        public string Username { get; set; }

        [Input]
        [RequireInput(HttpMethod.POST)]
        public string Password { get; set; }

        [Output]
        public Members Member { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsGet)
            {
                yield return this.ValidateSession();
                if (this.IsValidSession())
                {
                    yield return this.ValidateUsernameSession();
                }
            }

            if (this.IsPost)
            {
                if (this.IsValidRequireInputs(HttpMethod.POST))
                {
                    yield return this.ValidateUsernamePassword();
                }
            }
        }

        private ValidationResult ValidateUsernameSession()
        {
            var members = _Schemas.Members;
            members.Entity.username = this.SessionIdValue;
            members.Entity.active = (int)EnumActive.Active;
            if (members.Count == 0)
            {
                return TfValidationResult.Compose(
                    "SessionExpired",
                    nameof(this.Username),
                    nameof(this.Password));
            }
            return null;
        }

        private ValidationResult ValidateUsernamePassword()
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

        public override void MapModel()
        {
            var members = _Schemas.Members;
            members.Entity.username = this.SessionIdValue;
            members.Entity.active = (int)EnumActive.Active;
            var member = members.Select.Entity;
            this.Member = member;
            this.Member.password = null;
        }

        public override void HandleModel()
        {
            this.NewSession(this.Username);
        }
    }
}