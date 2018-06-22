using PrayerForums.Library;
using PrayerForums.Library.Database;
using PrayerForums.Library.Function.Interface;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Member
{
    public class LoginMemberLibrary : TfLibrary
    {
        internal ValidationResult ValidateUsernameSession(ILoginMember loginMember)
        {
            var members = _Schemas.Members;
            members.Entity.username = loginMember.SessionIdValue;
            members.Entity.active = EnumActive.Active;
            if (members.Count == 0)
            {
                return TfValidationResult.Compose(
                    "SessionExpired",
                    nameof(loginMember.Username),
                    nameof(loginMember.Password));
            }
            return null;
        }

        internal ValidationResult ValidateUsernamePassword(ILoginMember loginMember)
        {
            var members = _Schemas.Members;
            members.Entity.username = loginMember.Username;
            members.Entity.password = loginMember.Password;
            if (members.Count == 0)
            {
                return TfValidationResult.Compose(
                    "InvalidUsernamePassword",
                    nameof(loginMember.Username),
                    nameof(loginMember.Password));
            }
            return null;
        }

        internal void GetMember(ILoginMember loginMember)
        {
            var members = _Schemas.Members;
            members.Entity.username = loginMember.SessionIdValue;
            members.Entity.active = EnumActive.Active;
            var member = members.Select.Entity;
            loginMember.Member = member;
            loginMember.Member.password = null;
        }
    }
}