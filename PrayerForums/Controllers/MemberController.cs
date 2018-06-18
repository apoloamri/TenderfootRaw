using Microsoft.AspNetCore.Mvc;
using PrayerForums.Models.Member;
using Tenderfoot.Mvc;

namespace PrayerForums.Controllers
{
    [Route("member")]
    public class MemberController : TfController
    {
        [HttpPost]
        public JsonResult InsertMember()
        {
            this.Initiate<InsertMemberModel>(false);
            return this.Conclude();
        }

        [HttpPost("facebook")]
        public JsonResult InsertFacebookMember()
        {
            this.Initiate<InsertFacebookMemberModel>(false);
            return this.Conclude();
        }

        [HttpPost("activate")]
        public JsonResult ActivateMember()
        {
            this.Initiate<ActivateMemberModel>(false);
            return this.Conclude();
        }

        [HttpGet("login")]
        public JsonResult CheckLoginMember()
        {
            this.Initiate<LoginMemberModel>(false);
            return this.Conclude();
        }

        [HttpPost("login")]
        public JsonResult LoginMember()
        {
            this.Initiate<LoginMemberModel>(false);
            return this.Conclude();
        }
    }
}
