using PrayerForums.Library.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace PrayerForums.Models.Member
{
    public class InsertMemberModel : TfModel<InsertMemberBase>
    {
        [Input]
        public Members Member { get; set; } = new Members();

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.IsValid(this.Member.GetInputColumns()))
            {
                yield return this.Library.ValidateEmail();
                yield return this.Library.ValidateUsername();
            }
        }

        public override void HandleModel()
        {
            this.Library.InsertMember();
            this.Library.SendEmail();
        }
    }
}