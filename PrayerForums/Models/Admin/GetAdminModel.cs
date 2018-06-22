using PrayerForums.Function;
using PrayerForums.Library.Database;
using PrayerForums.Library.Function.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Admin
{
    public class GetAdminModel : TfModel<AdminModelLibrary>, IAdmin
    {
        [Output]
        public Devotions DevotionalMessage { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValidSession)
            {
                yield return this.Library.ValidateAdmin(this);
            }
        }

        public override void MapModel()
        {
            this.Library.GetDevotionalMessage(this);
        }
    }
}