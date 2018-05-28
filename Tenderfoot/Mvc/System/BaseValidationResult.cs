using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Tenderfoot.Mvc.System
{
    public static class BaseValidationResult
    {
        public static ValidationResult ValidateAlphabet(object value, string[] memberNames)
        {
            return Validate(
                "^[a-zA-Z]*$",
                value,
                memberNames);
        }

        public static ValidationResult ValidateAlphaNumeric(object value, string[] memberNames)
        {
            return Validate(
                "^[a-zA-Z0-9]*$",
                value,
                memberNames);
        }
        
        public static ValidationResult ValidateEmail(object value, string[] memberNames)
        {
            return Validate(
                @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", 
                value, 
                memberNames);
        }

        public static ValidationResult ValidateDateTime(object value, string[] memberNames)
        {
            if (!DateTime.TryParse(value?.ToString(), out DateTime dateTime))
            {
                TfValidationResult.Compose("InvalidInput", memberNames);
            }
            return null;
        }

        public static ValidationResult ValidateNumber(object value, string[] memberNames)
        {
            return Validate(
                @"^\d+$",
                value,
                memberNames);
        }

        public static ValidationResult ValidateString(object value, string[] memberNames)
        {
            return Validate(
                @"^\w+( \w+)*$",
                value,
                memberNames);
        }

        public static ValidationResult ValidateURL(object value, string[] memberNames)
        {
            return Validate(
                @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9]\.[^\s]{2,})",
                value,
                memberNames);
        }

        private static ValidationResult Validate(string pattern, object value, string[] memberNames)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (!regex.IsMatch(value?.ToString() ?? ""))
            {
                return TfValidationResult.Compose(
                    message: "InvalidInput",
                    values: memberNames,
                    memberNames: memberNames);
            }
            return null;
        }
    }
}
