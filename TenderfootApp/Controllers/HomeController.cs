using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootApp.Models.Home;

namespace TenderfootApp.Controllers
{
    [Route("home")]
    public class HomeController : TfController
    {
        [HttpGet]
        public JsonResult GetHome()
        {
            this.Initiate<HomeModel>();
            return this.Conclude();
        }
    }
}
