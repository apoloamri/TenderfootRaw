using PrayerForumsLibrary;
using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Interfaces;
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