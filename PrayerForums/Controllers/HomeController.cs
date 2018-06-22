using Microsoft.AspNetCore.Mvc;
using PrayerForums.Models.Home;
using Tenderfoot.Mvc;

namespace PrayerForums.Controllers
{
    [Route("home")]
    public class HomeController : TfController
    {
        [HttpGet]
        public JsonResult GetHome()
        {
            this.Initiate<HomeModel>(false);
            return this.Conclude();
        }
    }
}
