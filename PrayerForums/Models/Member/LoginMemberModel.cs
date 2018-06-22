using PrayerForums.Library.Database;
using PrayerForums.Library.Function.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Member
{
    public class LoginMemberModel : 
        TfModel<LoginMemberLibrary>,
        ILoginMember
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
                if (this.IsValidSession)
                {
                    yield return this.Library.ValidateUsernameSession(this);
                }
            }

            if (this.IsPost)
            {
                if (this.IsValidRequireInputs(HttpMethod.POST))
                {
                    yield return this.Library.ValidateUsernamePassword(this);
                }
            }
        }
        
        public override void MapModel()
        {
            this.Library.GetMember(this);
        }

        public override void HandleModel()
        {
            this.NewSession(this.Username);
        }
    }
}