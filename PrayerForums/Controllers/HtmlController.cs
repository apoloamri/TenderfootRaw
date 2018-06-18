using Microsoft.AspNetCore.Mvc;
using PrayerForums.Models.Prayer;
using Tenderfoot.Mvc;

namespace PrayerForums.Controllers
{
    public class HtmlController : TfController
    {
        [View("main")]
        [HttpGet("{viewName}.html")]
        public ActionResult GetView(string viewName)
        {
            return this.View(viewName);
        }

        [View("main")]
        [HttpGet("report.html")]
        public ActionResult GetReport()
        {
            this.Initiate<GetReportModel>(false);
            return this.View("report");
        }

        [HttpGet("signin.html")]
        public ActionResult GetSignIn()
        {
            return this.View("signin");
        }
    }
}
