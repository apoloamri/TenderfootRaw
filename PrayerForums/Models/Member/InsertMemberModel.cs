using PrayerForumsLibrary;
using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Member
{
    public class InsertMemberModel : 
        TfModel<InsertMemberLibrary>,
        IInsertMember
    {
        [Input]
        public Members Member { get; set; } = new Members();
        
        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValid(this.Member.GetInputColumns()))
            {
                yield return this.Library.ValidateEmail(this);
                yield return this.Library.ValidateUsername(this);
            }
        }
        
        public override void HandleModel()
        {
            this.Library.InsertMember(this);
            this.Library.SendEmail(this);
        }
    }
}