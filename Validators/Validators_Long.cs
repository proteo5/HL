using System;
using System.Collections.Generic;
using System.Text;

namespace Proteo5.HL.Validators
{
    public static class Validators_Long
    {
        public static ValidatorResult IsEqualTo(this long val, string itemName, string NotValidMessage, long value)
        {
            bool isEqualTo = val == value;

            string message = isEqualTo ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isEqualTo, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsNotEqualTo(this long val, string itemName, string NotValidMessage, long value)
        {
            bool isNotEqualTo = val != value;

            string message = isNotEqualTo ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isNotEqualTo, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsGreaterThan(this long val, string itemName, string NotValidMessage, long value)
        {
            bool isGraterThan = val > value;

            string message = isGraterThan ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isGraterThan, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsGreaterOrEqualThan(this long val, string itemName, string NotValidMessage, long value)
        {
            bool isGraterOrEqualThan = val >= value;

            string message = isGraterOrEqualThan ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isGraterOrEqualThan, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsLessThan(this long val, string itemName, string NotValidMessage, long value)
        {
            bool isLessThan = val < value;

            string message = isLessThan ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isLessThan, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsLessOrEqualThan(this long val, string itemName, string NotValidMessage, long value)
        {
            bool isLessOrEqualThan = val <= value;

            string message = isLessOrEqualThan ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isLessOrEqualThan, ItemName = itemName, Message = message };
        }
        public static ValidatorResult IsInBetween(this long val, string itemName, string NotValidMessage, long value1, long value2)
        {
            bool isInBetween = val >= value1 && val <= value2;

            string message = isInBetween ? "" : string.Format(NotValidMessage, itemName);
            return new ValidatorResult() { IsValid = isInBetween, ItemName = itemName, Message = message };
        }
    }
}
