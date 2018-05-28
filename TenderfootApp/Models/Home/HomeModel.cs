using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace TenderfootApp.Models.Home
{
    public class HomeModel : TfModel
    {
        [Output]
        public string Result { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            return null;
        }

        public override void MapModel()
        {
            this.Result = "Hello World.";
        }
    }
}