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
            return this.Page(viewName);
        }

        [View("main")]
        [HttpGet("report.html")]
        public ActionResult GetReport()
        {
            this.Initiate<GetReportModel>(false);
            return this.Page("report");
        }

        [View("main")]
        [HttpGet("details.html")]
        public ActionResult GetDetails()
        {
            this.Initiate<GetDetailsModel>(false);
            return this.Page("details");
        }

        [HttpGet("signin.html")]
        public ActionResult GetSignIn()
        {
            return this.Page("signin");
        }
    }
}
