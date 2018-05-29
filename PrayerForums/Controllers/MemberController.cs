using Microsoft.AspNetCore.Mvc;
using PrayerForums.Models.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost("activate")]
        public JsonResult ActivateMember()
        {
            this.Initiate<ActivateMemberModel>(false);
            return this.Conclude();
        }
    }
}
