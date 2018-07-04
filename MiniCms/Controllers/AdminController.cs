using Microsoft.AspNetCore.Mvc;
using MiniCms.Models.Admin;
using Tenderfoot.Mvc;

namespace MiniCms.Controllers
{
    [Route("admin")]
    [GetSession]
    public class AdminController : TfController
    {
        [View("layout")]
        [Route("login")]
        public ActionResult LoginPage()
        {
            this.Initiate<LoginModel>();
            return this.Page("login");
        }

        [HttpPost]
        [Route("login")]
        public ActionResult PostLogin()
        {
            this.Initiate<LoginModel>();
            if (!this.IsValid)
            {
                this.SetModel();
                return this.LoginPage();
            }
            return this.Page("dashboard");
        }

        [View("layout")]
        [Route("{pageName}")]
        public ActionResult AdminPages(string pageName)
        {
            this.Initiate<AdminPagesModel>();
            if (!this.IsValid)
            {
                return this.LoginPage();
            }
            return this.Page(pageName);
        }
    }
}
