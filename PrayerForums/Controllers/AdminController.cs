using Microsoft.AspNetCore.Mvc;
using PrayerForums.Models.Admin;
using Tenderfoot.Mvc;

namespace PrayerForums.Controllers
{
    [GetSession]
    [CheckActiveSession]
    [Route("ministry/admin")]
    public class AdminController : TfController
    {
        [HttpGet]
        [View("main")]
        public ActionResult GetAdmin()
        {
            this.Initiate<GetAdminModel>(false);
            return this.View("index");
        }

        [HttpGet("{pageName}")]
        [View("main")]
        public ActionResult GetAdminPages(string pageName)
        {
            return this.View(pageName);
        }

        [HttpPost("devote")]
        public JsonResult UpdateDevotion()
        {
            this.Initiate<UpdateDevotionModel>(false);
            return this.Conclude();
        }

        [HttpGet("requests/get")]
        public JsonResult GetRequests()
        {
            this.Initiate<GetRequestsModel>(false);
            return this.Conclude();
        }

        [HttpDelete("requests/delete")]
        public JsonResult DeleteRequests()
        {
            this.Initiate<DeleteRequestsModel>(false);
            return this.Conclude();
        }

        [HttpGet("praises/get")]
        public JsonResult GetPraises()
        {
            this.Initiate<GetPraisesModel>(false);
            return this.Conclude();
        }

        [HttpDelete("praises/delete")]
        public JsonResult DeletePraises()
        {
            this.Initiate<DeletePraisesModel>(false);
            return this.Conclude();
        }
    }
}
