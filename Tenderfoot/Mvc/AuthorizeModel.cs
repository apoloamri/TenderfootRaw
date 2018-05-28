using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Database.Tables;

namespace Tenderfoot.Mvc
{
    public class AuthorizeModel : TfModel
    {
        public Schema<Accesses> Accesses { get; set; } = Schemas.Accesses;
        
        [Input]
        [RequireInput(HttpMethod.POST)]
        [ValidateInput(InputType.String, 50)]
        public string Key { get; set; }
        
        [Input]
        [RequireInput(HttpMethod.POST)]
        [ValidateInput(InputType.String, 50)]
        public string Secret { get; set; }
        
        public override string SessionKey { get; set; }
        
        public override string SessionId { get; set; }

        [Output]
        public string Token { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Handling)
            {
                this.Accesses.Entity.key = this.Key;
                this.Accesses.Entity.secret = this.Secret;
                if (this.Accesses.Count == 0)
                {
                    yield return TfValidationResult.Compose("Unauthorized");
                }
            }
        }

        public override void HandleModel()
        {
            this.NewSession(this.Key);
            this.Token = this.SessionId + " " + this.SessionKey;
        }
    }
}
