using PrayerForums.Library;
using PrayerForums.Library.Database;
using PrayerForums.Library.Function.Interface;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.Net;
using Tenderfoot.Tools;

namespace PrayerForums.Models.Member
{
    public class InsertMemberLibrary : TfLibrary
    {
        internal ValidationResult ValidateEmail(IInsertMember insertMember)
        {
            var members = _Schemas.Members;
            members.Entity.email = insertMember.Member.email;
            members.Entity.active = EnumActive.Active;
            if (members.Count > 0)
            {
                return TfValidationResult.Compose(
                    "InvalidDuplicate",
                    new[] { insertMember.Member.email },
                    nameof(insertMember.Member.email));
            }
            return null;
        }

        internal ValidationResult ValidateUsername(IInsertMember insertMember)
        {
            var members = _Schemas.Members;
            members.Entity.username = insertMember.Member.username;
            members.Entity.active = EnumActive.Active;
            if (members.Count > 0)
            {
                return TfValidationResult.Compose(
                    "InvalidDuplicate",
                    new[] { insertMember.Member.username },
                    nameof(insertMember.Member.username));
            }
            return null;
        }

        internal void InsertMember(IInsertMember insertMember)
        {
            insertMember.Member.activation_key = KeyGenerator.GetUniqueKey(100);
            var members = _Schemas.Members;
            members.Entity.SetValuesFromModel(insertMember.Member, false);
            members.Insert();
        }

        internal void SendEmail(IInsertMember insertMember)
        {
            TfEmail.Send(
                "MemberRegister",
                insertMember.Member.email,
                insertMember.Member.ToDictionary());
        }
    }
}