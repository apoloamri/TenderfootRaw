using PrayerForums.Library;
using PrayerForums.Library.Database;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.Net;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools;

namespace PrayerForums.Models.Member
{
    public class InsertMemberBase : TfBaseModel
    {
        public Members Member { get; set; }

        internal ValidationResult ValidateEmail()
        {
            var members = _Schemas.Members;
            members.Entity.email = this.Member.email;
            members.Entity.active = EnumActive.Active;
            if (members.Count > 0)
            {
                return TfValidationResult.Compose(
                    "InvalidDuplicate",
                    new[] { this.Member.email },
                    nameof(this.Member.email));
            }
            return null;
        }

        internal ValidationResult ValidateUsername()
        {
            var members = _Schemas.Members;
            members.Entity.username = this.Member.username;
            members.Entity.active = EnumActive.Active;
            if (members.Count > 0)
            {
                return TfValidationResult.Compose(
                    "InvalidDuplicate",
                    new[] { this.Member.username },
                    nameof(this.Member.username));
            }
            return null;
        }

        internal void InsertMember()
        {
            this.Member.activation_key = KeyGenerator.GetUniqueKey(100);
            var members = _Schemas.Members;
            members.Entity.SetValuesFromModel(this.Member, false);
            members.Insert();
        }

        internal void SendEmail()
        {
            TfEmail.Send(
                "MemberRegister",
                this.Member.email,
                this.Member.ToDictionary());
        }
    }
}