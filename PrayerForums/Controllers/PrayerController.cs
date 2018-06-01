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

        [HttpGet("requests")]
        public JsonResult GetRequests()
        {
            this.Initiate<GetRequestsModel>(false);
            return this.Conclude();
        }

        [HttpGet("details")]
        public JsonResult GetDetails()
        {
            this.Initiate<GetDetailsModel>(false);
            return this.Conclude();
        }
    }
}
