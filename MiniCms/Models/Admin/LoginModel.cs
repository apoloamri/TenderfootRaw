using MiniCmsLibrary;
using MiniCmsLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace MiniCms.Models.Admin
{
    public class LoginModel : TfModel<LoginLibrary>, ILogin
    {
        [Input(InputType.All)]
        [RequireInput(HttpMethod.POST)]
        public string username { get; set; }

        [Input(InputType.All)]
        [RequireInput(HttpMethod.POST)]
        public string password { get; set; }
        
        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Handling)
            {
                yield return this.Library.ValidateLogin(this);
            }
        }

        public override void HandleModel()
        {
            this.Library.StartSession(this);
        }
    }
}