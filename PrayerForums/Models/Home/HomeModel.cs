using PrayerForumsLibrary;
using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Home
{
    public class HomeModel : TfModel<AdminModelLibrary>, IAdmin
    {
        [Output]
        public Devotions DevotionalMessage { get; set; }
        
        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }
        
        public override void MapModel()
        {
            this.Library.GetDevotionalMessage(this);
        }
    }
}