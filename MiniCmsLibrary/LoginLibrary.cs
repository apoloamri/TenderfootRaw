using MiniCmsLibrary.Interfaces;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Mvc;

namespace MiniCmsLibrary
{
    public class LoginLibrary : TfLibrary
    {
        public ValidationResult ValidateLogin(ILogin login)
        {
            var accounts = _DB.Accounts;
            accounts.Entity.username = login.username;
            accounts.Entity.password = login.password;
            accounts.Entity.admin = Enums.EnumAdmin.Admin;
            if (accounts.Count == 0)
            {
                return TfValidationResult.Compose(
                    "InvalidUsernamePassword", 
                    nameof(login.username), 
                    nameof(login.password));
            }
            return null;
        }

        public void StartSession(ILogin login)
        {
            login.NewSession(login.username);
            login.SetSessionCookies();
        }
    }
}