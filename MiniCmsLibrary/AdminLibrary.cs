using MiniCmsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace MiniCmsLibrary
{
    public class AdminLibrary : TfLibrary
    {
        public override IEnumerable<ValidationResult> AutoValidate<I>(I model)
        {
            yield return this.ValidateAdmin((IAdmin)model);
        }

        public ValidationResult ValidateAdmin(IAdmin admin)
        {
            var accounts = _DB.Accounts;
            accounts.Entity.username = admin.SessionIdValue;
            accounts.Entity.admin = Enums.EnumAdmin.Admin;
            if (accounts.Count == 0)
            {
                return TfValidationResult.Compose("SessionExpired", nameof(admin.SessionId));
            }
            return null;
        }
    }
}