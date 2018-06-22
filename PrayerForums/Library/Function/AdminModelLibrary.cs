using PrayerForums.Library;
using PrayerForums.Library.Database;
using PrayerForums.Library.Function.Interface;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace PrayerForums.Function
{
    public class AdminModelLibrary : TfLibrary
    {
        public ValidationResult ValidateAdmin(IAdmin admin)
        {
            var member = _Schemas.Members;
            member.Entity.username = admin.SessionIdValue;
            member.Entity.admin = EnumAdmin.Admin;
            if (member.Count == 1)
            {
                return null;
            }
            return TfValidationResult.Compose("SessionExpired");
        }

        public void GetDevotionalMessage(IAdmin admin)
        {
            var devotions = _Schemas.Devotions;
            devotions.Case.OrderBy(devotions.Column(x => x.insert_time), OrderBy.DESC);
            devotions.Case.LimitBy(1);
            admin.DevotionalMessage = devotions.Select.Entity;
        }
    }
}