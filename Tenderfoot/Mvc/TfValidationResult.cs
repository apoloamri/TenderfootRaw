using Tenderfoot.TfSystem;
using Tenderfoot.Tools.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tenderfoot.Mvc.System;

namespace Tenderfoot.Mvc
{
    public static class TfValidationResult
    {
        public static ValidationResult CheckSessionActivity(string sessionId, string sessionKey, params string[] memberNames)
        {
            if (Session.IsSessionActive(sessionId, sessionKey))
            {
                return null;
            }

            return Compose("SessionExpired", "SessionKey");
        }

        public static ValidationResult Compose(string message, params string[] memberNames)
        {
            return Compose(message, null, memberNames);
        }

        public static ValidationResult Compose(string message, string[] values, params string[] memberNames)
        {
            var newMessage = Resources.GetMessage(message, values);

            if (memberNames != null && memberNames.Count() > 0)
            {
                return new ValidationResult(newMessage, memberNames.Select(x => x.ToUnderscore()));
            }

            return new ValidationResult(newMessage, new[] { "common" });
        }

        public static ValidationResult FieldRequired(string fieldName, object value, string message = null)
        {
            if (message.IsEmpty())
            {
                message = Resources.GetMessage("RequiredField", fieldName);
            }

            if (value == null)
            {
                return Compose(message, fieldName);
            }
            else
            {
                if (value.ToString().IsEmpty())
                {
                    return Compose(message, fieldName);
                }
            }

            return null;
        }

        public static ValidationResult ValidateInput(InputType inputType, object value, params string[] memberNames)
        {
            switch (inputType)
            {
                case InputType.Alphabet:
                    return BaseValidationResult.ValidateAlphabet(value, memberNames);
                case InputType.AlphaNumeric:
                    return BaseValidationResult.ValidateAlphaNumeric(value, memberNames);
                case InputType.Email:
                    return BaseValidationResult.ValidateEmail(value, memberNames);
                case InputType.DateTime:
                    return BaseValidationResult.ValidateDateTime(value, memberNames);
                case InputType.Numeric:
                    return BaseValidationResult.ValidateNumber(value, memberNames);
                case InputType.String:
                    return BaseValidationResult.ValidateString(value, memberNames);
                case InputType.URL:
                    return BaseValidationResult.ValidateURL(value, memberNames);
                default:
                    return null;
            }
        }
    }
}
