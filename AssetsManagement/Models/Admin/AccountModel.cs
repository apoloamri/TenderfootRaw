using AssetsManagement.Lib.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.Tools.Extensions;

namespace AssetsManagement.Models.Admin
{
    class AccountModel : TfModel
    {
        [Input(InputType.String)]
        [RequireInput(HttpMethod.POST)]
        public string username { get; set; }

        [Input(InputType.String)]
        [RequireInput(HttpMethod.POST)]
        public string password { get; set; }

        [Input(InputType.All)]
        public string[] page_access { get; set; }

        [Input(InputType.Boolean)]
        [RequireInput(HttpMethod.POST)]
        public bool accounting { get; set; }

        public override void HandleModel()
        {
            if (this.page_access == null)
            {
                this.page_access = new string[0];
            }
            var admins = _DB.Admins;
            var pageAccess = this.page_access.ToList();
            pageAccess.Add("/assets/getall");
            this.page_access = pageAccess.ToArray();
            admins.Entity.SetValuesFromModel(this);
            admins.Insert();
        }

        [Output]
        public List<Dictionary<string, object>> List { get; set; } = new List<Dictionary<string, object>>();

        public override void MapModel()
        {
            var admins = _DB.Admins;
            admins.Case.OrderBy(admins.Column(x => x.id), OrderBy.ASC);
            foreach (var admin in admins.Select.Entities)
            {
                var dictionary = admin.ToDictionary();
                var pageAccessList = this.GetPageAccessList(admin.page_access);
                dictionary["page_access"] = string.Join(", ", pageAccessList);
                List.Add(dictionary);
            }
        }

        private IEnumerable<string> GetPageAccessList(string[] page_access)
        {
            foreach (var page in page_access)
            {
                switch (page)
                {
                    case "/assets/add":
                        yield return "NEW ASSET";
                        break;
                    case "/assets/getall":
                        yield return "ASSET LIST";
                        break;
                    case "/assets/update":
                        yield return "UPDATE ASSET";
                        break;
                    case "/options":
                        yield return "OPTIONS";
                        break;
                    case "/accounts":
                        yield return "ACCOUNTS";
                        break;
                }
            }
        }

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.ValidateSession();
            if (this.Handling)
            {
                if (this.IsValidRequireInputs() &&
                    this.IsValidSession)
                {
                    var admins = _DB.Admins;
                    admins.Entity.username = this.username;
                    if (admins.Count != 0)
                    {
                        yield return TfValidationResult.Compose(
                            "InvalidDuplicate",
                            new[] { this.username },
                            nameof(this.username));
                    }
                }
            }
        }
    }
}