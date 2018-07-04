using PrayerForumsLibrary.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;
using Tenderfoot.Net;

namespace PrayerForums.Models.Prayer
{
    public class InsertRequestModel : TfModel
    {
        [Input]
        [Output]
        public Requests Request { get; set; } = new Requests();
        
        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void HandleModel()
        {
            var requests = _Schemas.Requests;
            requests.Entity.SetValuesFromModel(this.Request);
            requests.Insert();
            TfEmail.Send(
                "NewPrayerRequest",
                this.Request.email,
                this.Request.ToDictionary());
        }
    }
}