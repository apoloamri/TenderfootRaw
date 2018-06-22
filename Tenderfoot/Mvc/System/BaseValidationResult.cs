using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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

        public static ValidationResult ValidateBoolean(object value, string[] memberNames)
        {
            if (value is bool ||
                (value as string) == "true" ||
                (value as string) == "false" ||
                (value as int?) == 1 ||
                (value as int?) == 0)
            {
                return null;
            }
            return TfValidationResult.Compose("InvalidInput", memberNames, memberNames);
        }

        public static ValidationResult ValidateEmail(object value, string[] memberNames)
        {
            return Validate(
                @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", 
                value, 
                memberNames);
        }

        public static ValidationResult ValidateEnum(object value, string[] memberNames)
        {
            var list = new List<int>();
            foreach (var item in Enum.GetValues(value.GetType()))
            {
                list.Add((int)item);
            }

            if (!list.Contains((int)value))
            {
                return TfValidationResult.Compose("InvalidInput", memberNames, memberNames);
            }
            return null;
        }

        public static ValidationResult ValidateFileName(object value, string[] memberNames)
        {
            return Validate(
                @"^[\w,\s-]+\.[A-Za-z]{3,4}$",
                value,
                memberNames);
        }

        public static ValidationResult ValidateFilePath(object value, string[] memberNames)
        {
            if (!Directory.Exists(Path.GetDirectoryName(Convert.ToString(value))))
            {
                return TfValidationResult.Compose("InvalidInput", memberNames, memberNames);
            }
            return null;
        }

        public static ValidationResult ValidateDateTime(object value, string[] memberNames)
        {
            if (!DateTime.TryParse(value?.ToString(), out DateTime dateTime))
            {
                return TfValidationResult.Compose("InvalidInput", memberNames, memberNames);
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
            if (!Uri.IsWellFormedUriString(value.ToString(), UriKind.RelativeOrAbsolute))
            {
                return TfValidationResult.Compose("InvalidInput", memberNames, memberNames);
            }
            return null;
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
