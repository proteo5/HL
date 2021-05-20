﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Proteo5.HL.Validators
{
    public static class Validators_String
    {
        public static string Clean(this string str)
        {
            str = str ?? "";
            str = str.Replace("\r", "")
                     .Replace("\n", "")
                     .Replace("\t", "")
                     .Replace("\b", "")
                     .Replace("\a", "")
                     .Replace("\f", "")
                     .Replace("\v", "")
                     .Trim();
            return str;
        }

        public static string CleanPhone(this string str)
        {
            str = str.Clean()
                     .CleanNumber()
                     .Replace("(", "")
                     .Replace(")", "")
                     .Replace(" ", "")
                     .Replace("-", "");
            return str;
        }

        public static string CleanNumber(this string str)
        {
            str = str.Clean()
                     .Replace(",", "")
                     .Replace("$", "");
            return str;
        }
        public static string toUrlText(this string str)
        {
            str = str.Clean().Replace(" ", "_").RemoveDiacritics().ToLower();
            return str;
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static ValidatorResult Required(this string str, string itemName, string NotValidMessage)
        {
            bool result = !string.IsNullOrWhiteSpace(str);
            string message = result ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = result, ItemName = itemName, Message = message };
        }

        public static ValidatorResult Required(this string str, string itemName, string NotValidMessage, RequiredOnlyTextType onlyTextType)
        {
            bool IsValid = true;
            if (str.Length != 0)
            {
                switch (onlyTextType)
                {
                    case RequiredOnlyTextType.OnlyLetters:
                        IsValid = Regex.IsMatch(str, @"^[a-zA-Z]+$");
                        break;
                    case RequiredOnlyTextType.OnlyLettersAndNumbers:
                        IsValid = Regex.IsMatch(str, @"^[a-zA-Z0-9]+$");
                        break;
                    case RequiredOnlyTextType.OnlyLettersNumbersAndUnderscore:
                        IsValid = Regex.IsMatch(str, @"^[a-zA-Z0-9_]+$");
                        break;
                    case RequiredOnlyTextType.OnlyNumbers:
                        IsValid = Regex.IsMatch(str, @"^[0-9]+$");
                        break;
                }
            }

            string message = IsValid ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = IsValid, ItemName = itemName, Message = message };
        }

        public static ValidatorResult IsEmail(this string str, string itemName, string NotValidMessage)
        {
            bool IsValid;

            var required = str.Required(itemName, NotValidMessage);
            if (!required.IsValid)
                return required;

            try
            {
                MailAddress m = new MailAddress(str);

                IsValid = true;
            }
            catch (FormatException)
            {
                IsValid = false;
            }

            string message = IsValid ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = IsValid, ItemName = itemName, Message = message };
        }

        public static ValidatorResult IsLength(this string str, string itemName, string NotValidMessage, int Min = 0, int Max = int.MaxValue)
        {

            bool result2 = str.Length == 0 || (str.Length >= Min && str.Length <= Max);

            string message = result2 ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = result2, ItemName = itemName, Message = message };
        }

        public static ValidatorResult IsEquals(this string str, string itemName, string NotValidMessage, string Compare)
        {
            if (str == null) str = "";
            if (Compare == null) Compare = "";
            bool result2 = str.Equals(Compare);

            string message = result2 ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = result2, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsNotEquals(this string str, string itemName, string NotValidMessage, string Compare)
        {
            bool result2 = !str.Equals(Compare);

            string message = result2 ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = result2, ItemName = itemName, Message = message };
        }

        public static ValidatorResult IsDecimal(this string str, string itemName, string NotValidMessage, out decimal dec)
        {
            str = str == "" ? "0" : str;
            bool isDecimal = decimal.TryParse(str, out dec);

            string message = isDecimal ? "" : string.Format(NotValidMessage, itemName);
            dec = isDecimal ? dec : 0m;
            return new ValidatorResult() { IsValid = isDecimal, ItemName = itemName, Message = message };
        }

        public static ValidatorResult IsInteger(this string str, string itemName, string NotValidMessage, out int integer)
        {
            str = str == "" ? "0" : str;
            bool isInteger = int.TryParse(str, out integer);

            string message = isInteger ? "" : string.Format(NotValidMessage, itemName);
            integer = isInteger ? integer : 0;
            return new ValidatorResult() { IsValid = isInteger, ItemName = itemName, Message = message };
        }

    }

}