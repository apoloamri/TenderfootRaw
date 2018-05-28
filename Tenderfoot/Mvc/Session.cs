using Tenderfoot.Database;
using Tenderfoot.TfSystem;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;
using System;

namespace Tenderfoot.Mvc
{
    public static class Session
    {
        public static string AddSession(string sessionId, out string sessionKey)
        {
            sessionKey = string.Empty;
            
            if (sessionId.IsEmpty())
            {
                return string.Empty;
            }

            var session = Schemas.Sessions;
            session.Case.Where(session._(x => x.session_id), Is.EqualTo, sessionId);
            session.Case.Where(session._(x => x.session_time), Is.GreaterThan, DateTime.Now.AddMinutes(-TfSettings.Web.SessionTimeOut));

            var result = session.Select.Entity;

            if (result != null)
            {
                sessionKey = result.session_key;
                return Encryption.Encrypt(sessionId);
            }
            
            do
            {
                sessionKey = KeyGenerator.GetUniqueKey(64);
                session.ClearCase();
                session.Entity.session_key = sessionKey;
            }
            while (session.Count > 0);

            session.Entity.session_id = sessionId;
            session.Entity.session_time = DateTime.Now;
            session.Insert();

            return Encryption.Encrypt(sessionId);
        }

        public static bool IsSessionActive(string sessionId, string sessionKey)
        {
            if (sessionId.IsEmpty() || sessionKey.IsEmpty())
            {
                return false;
            }

            var session = Schemas.Sessions;
            session.Case.Where(session._(x => x.session_id), Is.EqualTo, Encryption.Decrypt(sessionId));
            session.Case.Where(session._(x => x.session_key), Is.EqualTo, sessionKey);
            session.Case.Where(session._(x => x.session_time), Is.GreaterThan, DateTime.Now.AddMinutes(-TfSettings.Web.SessionTimeOut));
            
            var count = session.Count;
            if (count > 0)
            {
                session.Entity.session_time = DateTime.Now;
                session.Update();
                return true;
            }

            return false;
        }
    }
}
