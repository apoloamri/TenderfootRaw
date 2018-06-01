using PrayerForums.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Member
{
    public class LoginMemberModel : TfModel<LoginMemberBase>
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
                    yield return this.Library.ValidateUsernameSession(this.SessionIdValue);
                }
            }

            if (this.IsPost)
            {
                if (this.IsValidRequireInputs(HttpMethod.POST))
                {
                    yield return this.Library.ValidateUsernamePassword();
                }
            }
        }
        
        public override void MapModel()
        {
            this.Library.GetMember(this.SessionIdValue);
        }

        public override void HandleModel()
        {
            this.NewSession(this.Username);
        }
    }
}