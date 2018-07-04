using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.Net;

namespace PrayerForums.Models.Member
{
    public class ActivateMemberModel : TfModel
    {
        [Input]
        [RequireInput(HttpMethod.POST)]
        public string Key { get; set; }
        
        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValid(this.Key))
            {
                var members = _Schemas.Members;
                members.Entity.activation_key = this.Key;
                members.Entity.active = (int)EnumActive.Inactive;
                if (members.Count == 0)
                {
                    yield return TfValidationResult.Compose("InvalidActivation", nameof(this.Key));
                }
            }
        }

        public override void HandleModel()
        {
            var members = _Schemas.Members;
            members.Entity.active = EnumActive.Active;
            members.Case.Where(members.Column(x => x.activation_key), Is.EqualTo, this.Key);
            members.Update();
            TfEmail.Send(
                "MemberActivated",
                members.SelectToEntity().email,
                members.Entity.ToDictionary());
        }
    }
}