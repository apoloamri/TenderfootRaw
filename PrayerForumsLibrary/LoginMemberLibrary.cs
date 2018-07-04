using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Enums;
using PrayerForumsLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForumsLibrary
{
    public class LoginMemberLibrary : TfLibrary
    {
        public ValidationResult ValidateUsernameSession(ILoginMember loginMember)
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

        public ValidationResult ValidateUsernamePassword(ILoginMember loginMember)
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

        public void GetMember(ILoginMember loginMember)
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