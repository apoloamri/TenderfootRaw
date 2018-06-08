using Microsoft.AspNetCore.Mvc;
using PrayerForums.Models.Prayer;
using Tenderfoot.Mvc;

namespace PrayerForums.Controllers
{
    [Route("prayer")]
    public class PrayerController : TfController
    {
        [HttpPost("request")]
        public JsonResult InsertRequest()
        {
            this.Initiate<InsertRequestModel>(false);
            return this.Conclude();
        }

        [HttpPost("reply")]
        public JsonResult InsertReply()
        {
            this.Initiate<InsertReplyModel>(false);
            return this.Conclude();
        }

        [HttpPost("praise")]
        public JsonResult InsertPraise()
        {
            this.Initiate<InsertPraiseModel>(false);
            return this.Conclude();
        }

        [HttpGet("requests")]
        public JsonResult GetRequests()
        {
            this.Initiate<GetRequestsModel>(false);
            return this.Conclude();
        }

        [HttpGet("praises")]
        public JsonResult GetPraises()
        {
            this.Initiate<GetPraisesModel>(false);
            return this.Conclude();
        }

        [HttpGet("details")]
        public JsonResult GetDetails()
        {
            this.Initiate<GetDetailsModel>(false);
            return this.Conclude();
        }

        [HttpGet("report")]
        public JsonResult GetReport()
        {
            this.Initiate<GetReportModel>(false);
            return this.Conclude();
        }
    }
}
