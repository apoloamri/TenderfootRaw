using PrayerForums.Library;
using PrayerForums.Library.Database;
using System.Collections.Generic;
using Tenderfoot.Net;
using Tenderfoot.TfSystem;

namespace PrayerForums.Models.Prayer
{
    public class InsertReplyBase : TfBaseModel
    {
        public Replies Response { get; set; }
        public GetDetailsBase DetailsBase { get; set; }

        internal void InsertReply()
        {
            this.DetailsBase.GetRequest();
            var response = _Schemas.Replies;
            response.Entity.SetValuesFromModel(this.Response);
            response.Entity.request_id = this.Response.request_id;
            response.Insert();
            if (this.DetailsBase.Result.send_email == EnumActive.Active)
            {
                TfEmail.Send(
                    "NewPrayerResponse",
                    this.DetailsBase.Result.email,
                    this.CreateEmailDictionary(response.Entity));
            }
        }

        private Dictionary<string, object> CreateEmailDictionary(Replies reply)
        {
            return new Dictionary<string, object>
            {
                ["lastname"] = this.DetailsBase.Result.lastname,
                ["firstname"] = this.DetailsBase.Result.firstname,
                ["email"] = this.DetailsBase.Result.email,
                ["response"] = reply.response,
                ["sender_lastname"] = reply.lastname,
                ["sender_firstname"] = reply.firstname
            };
        }
    }
}