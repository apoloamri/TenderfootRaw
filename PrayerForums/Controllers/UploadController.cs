using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;

namespace PrayerForums.Controllers
{
    [Route("upload")]
    public class UploadController : TfController
    {
        [HttpPost("image/praise")]
        public JsonResult UploadImagePraise(IFormFile file)
        {
            this.Upload(new TfUploadModel()
            {
                FilePath = Convert.ToString(TfSettings.GetSettings("PrayerForums", "ImageUploadTemp")),
                File = file
            }, false);
            return this.Conclude();
        }
    }
}
