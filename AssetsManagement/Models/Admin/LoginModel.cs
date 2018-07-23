using AssetsManagement.Lib.DB;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tenderfoot.Mvc;

namespace AssetsManagement.Models.Admin
{
    class LoginModel : TfModel
    {
        [Input(InputType.String)]
        [RequireInput(HttpMethod.POST)]
        public string username { get; set; }

        [Input(InputType.String)]
        [RequireInput(HttpMethod.POST)]
        public string password { get; set; }
        
        public override void HandleModel()
        {
            this.NewSession(this.username);
        }

        [Output]
        [Input(InputType.All)]
        [RequireInput(HttpMethod.GET)]
        public override string SessionId { get; set; }

        [Output]
        [Input(InputType.All)]
        [RequireInput(HttpMethod.GET)]
        public override string SessionKey { get; set; }
        
        [Input(InputType.All)]
        [RequireInput(HttpMethod.GET)]
        public string PageAccess { get; set; }

        [Output]
        public string[] PageAccesses { get; set; }

        [Output]
        public bool IsAccounting { get; set; }

        public override void MapModel()
        {
            var admins = _DB.Admins;
            admins.Entity.username = this.SessionIdValue;
            this.PageAccesses = admins.SelectToEntity().page_access;
            this.IsAccounting = admins.Entity.accounting ?? false;
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Handling)
            {
                if (this.IsValidRequireInputs(HttpMethod.POST))
                {
                    var admins = _DB.Admins;
                    admins.Entity.SetValuesFromModel(this);
                    if (admins.Count == 0)
                    {
                        yield return TfValidationResult.Compose("InvalidUsernamePassword", nameof(this.username));
                    }
                }
            }

            if (this.Mapping)
            {
                if (this.IsValidRequireInputs(HttpMethod.GET))
                {
                    yield return this.ValidateSession();
                    var admins = _DB.Admins;
                    admins.Entity.username = this.SessionIdValue;
                    if (admins.Count == 0)
                    {
                        yield return TfValidationResult.Compose("InvalidSession", nameof(this.username));
                    }
                    else
                    {
                        admins.SelectToEntity();
                        var lastSlash = this.PageAccess.LastIndexOf('/');
                        var pageRoute = 
                            (lastSlash > -1) && this.PageAccess.Any(char.IsDigit) ? 
                            this.PageAccess.Substring(0, lastSlash) : 
                            this.PageAccess;
                        if (!admins.Entity.page_access.Contains(pageRoute))
                        {
                            yield return TfValidationResult.Compose("InvalidSession", nameof(this.username));
                        }
                    }
                }
            }
        }
    }
}