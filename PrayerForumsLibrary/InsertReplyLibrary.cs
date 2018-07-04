using PrayerForumsLibrary.Database;
using PrayerForumsLibrary.Enums;
using PrayerForumsLibrary.Interfaces;
using System.Collections.Generic;
using Tenderfoot.Mvc;
using Tenderfoot.Net;

namespace PrayerForumsLibrary
{
    public class InsertReplyLibrary : TfLibrary
    {
        public void InsertReply(IInsertReply insertReply)
        {
            new GetDetailsLibrary().GetRequest(insertReply);
            var response = _Schemas.Replies;
            response.Entity.SetValuesFromModel(insertReply.Response);
            response.Entity.request_id = insertReply.Response.request_id;
            response.Insert();
            if (insertReply.Requests.send_email == EnumActive.Active)
            {
                TfEmail.Send(
                    "NewPrayerResponse",
                    insertReply.Requests.email,
                    this.CreateEmailDictionary(response.Entity, insertReply.Requests));
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