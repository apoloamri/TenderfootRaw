using AssetsManagement.Lib.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace AssetsManagement.Models.Admin
{
    class DeleteAccountModel : TfModel
    {
        [Input(InputType.Numeric)]
        [RequireInput]
        public int? Id { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.ValidateSession();
            if (this.IsValidRequireInputs())
            {
                var admins = _DB.Admins;
                if (admins.Count < 2)
                {
                    yield return TfValidationResult.Compose("Cannot delete remaining account.", nameof(this.Id));
                }
                admins.Entity.id = this.Id;
                if (admins.Count == 0)
                {
                    yield return TfValidationResult.Compose("DataNotFound", nameof(this.Id));
                }
            }
        }

        public override void HandleModel()
        {
            var admins = _DB.Admins;
            admins.Entity.id = this.Id;
            admins.Delete();
        }
    }
}