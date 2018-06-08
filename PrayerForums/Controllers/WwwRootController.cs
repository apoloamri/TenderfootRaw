using Microsoft.AspNetCore.Mvc;
using System;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;

namespace PrayerForums.Controllers
{
    [Route("wwwroot")]
    public class WwwRootController : TfController
    {
        [HttpGet("images/praise/{name}")]
        public ActionResult ImagesPraise(string name)
        {
            return this.GetFile(
                Convert.ToString(TfSettings.GetSettings("PrayerForums", "ImageUploadPraise")),
                name,
                "image/jpeg");
        }
    }
}
