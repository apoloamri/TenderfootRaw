using AssetsManagement.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;

namespace AssetsManagement.Controllers
{
    [Route("admin")]
    public class AdminController : TfController
    {
        [HttpGet("login")]
        public ActionResult GetLogin()
        {
            this.Initiate<LoginModel>();
            return this.Conclude();
        }

        [HttpPost("login")]
        public ActionResult PostLogin()
        {
            this.Initiate<LoginModel>();
            return this.Conclude();
        }

        [HttpGet("account")]
        public ActionResult GetAccount()
        {
            this.Initiate<AccountModel>();
            return this.Conclude();
        }

        [HttpPost("account")]
        public ActionResult PostAccount()
        {
            this.Initiate<AccountModel>();
            return this.Conclude();
        }

        [HttpDelete("account")]
        public ActionResult DeleteAccount()
        {
            this.Initiate<DeleteAccountModel>();
            return this.Conclude();
        }
    }
}
