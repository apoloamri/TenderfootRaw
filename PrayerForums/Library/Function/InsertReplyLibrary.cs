using PrayerForums.Library.Database;
using PrayerForums.Models.Prayer;
using PrayerForums.Models.Prayer.Interface;
using System.Collections.Generic;
using Tenderfoot.Mvc;
using Tenderfoot.Net;

namespace PrayerForums.Library.Function
{
    public class InsertReplyLibrary : TfLibrary
    {
        internal void InsertReply(IInsertReply insertReply)
        {
            var getDetailsModel = new GetDetailsModel
            {
                RequestId = insertReply.Response.request_id
            };
            new GetDetailsLibrary().GetRequest(getDetailsModel);
            var response = _Schemas.Replies;
            response.Entity.SetValuesFromModel(insertReply.Response);
            response.Entity.request_id = insertReply.Response.request_id;
            response.Insert();
            if (getDetailsModel.Requests.send_email == EnumActive.Active)
            {
                TfEmail.Send(
                    "NewPrayerResponse",
                    getDetailsModel.Requests.email,
                    this.CreateEmailDictionary(response.Entity, getDetailsModel.Requests));
            }
        }

        private Dictionary<string, object> CreateEmailDictionary(Replies reply, Requests requests)
        {
            return new Dictionary<string, object>
            {
                ["lastname"] = requests.lastname,
                ["firstname"] = requests.firstname,
                ["email"] = requests.email,
                ["response"] = reply.response,
                ["sender_lastname"] = reply.lastname,
                ["sender_firstname"] = reply.firstname
            };
        }
    }
}