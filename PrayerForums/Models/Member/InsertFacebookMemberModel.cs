using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Member
{
    public class InsertFacebookMemberModel : TfModel
    {
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.String)]
        public string UserId { get; set; }

        [RequireInput(HttpMethod.POST)]
        [Input(InputType.Email)]
        public string Email { get; set; }
        
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.String)]
        public string Lastname { get; set; }
        
        [RequireInput(HttpMethod.POST)]
        [Input(InputType.String)]
        public string Firstname { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void HandleModel()
        {
            var members = _Schemas.Members;
            var facebook = _Schemas.Facebook;
            facebook.Entity.user_id = this.UserId;
            if (facebook.Count > 0)
            {
                members.Entity.id = facebook.SelectToEntity().member_id;
                if (members.Count > 0)
                {
                    this.NewSession(members.SelectToEntity().username);
                }
            }
            else
            {
                members.Entity.email = this.Email;
                if (members.Count > 0)
                {
                    facebook.Entity.member_id = members.SelectToEntity().id;
                    facebook.Insert();
                }
                else
                {
                    members.Entity.username = this.UserId;
                    members.Entity.password = "";
                    members.Entity.active = EnumActive.Active;
                    members.Entity.SetValuesFromModel(this);
                    members.Insert();
                    facebook.Entity.member_id = members.Entity.id;
                    facebook.Insert();
                }
                this.NewSession(members.Entity.username);
            }
        }
    }
}