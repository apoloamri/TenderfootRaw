using PrayerForumsLibrary;
using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Admin
{
    public class UpdateDevotionModel : 
        TfModel<AdminModelLibrary>,
        IAdmin
    {
        [Input]
        public Devotions Devotion { get; set; } = new Devotions();

        [Output]
        public Devotions DevotionalMessage { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValidSession)
            {
                yield return this.Library.ValidateAdmin(this);
            }
        }

        public override void HandleModel()
        {
            var devotion = _Schemas.Devotions;
            devotion.Entity.SetValuesFromModel(this.Devotion);
            devotion.Insert();
            this.DevotionalMessage = devotion.Entity;
        }
    }
}