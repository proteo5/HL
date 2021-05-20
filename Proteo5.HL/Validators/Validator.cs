using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proteo5.HL.Validators
{
    public class Validator
    {
        public List<ValidatorResult> Items = new List<ValidatorResult>();

        public void Add(ValidatorResult validatorResult)
        {
            this.Items.Add(validatorResult);
        }

        public void Clear(ValidatorResult validatorResult)
        {
            this.Items.Clear();
        }

        public Result Validate()
        {
            ValidatorResults validatorResults = new ValidatorResults();
            foreach (var item in Items)
            {
                if (!item.IsValid)
                {
                    validatorResults.IsValid = false;
                    validatorResults.InvalidItems.Add(item);
                }
            }

            if (validatorResults.IsValid)
            {
                return new Result { State = ResultsStates.success };
            }
            else
            {
                return new Result { State = ResultsStates.invalid, ValidationResults = validatorResults };
            }
        }

        public bool ValidateItem(string item)
        {
            bool isValid = true;
            var ItemsFiltered = this.Items.Where(i => i.ItemName == item);
            foreach (var _item in ItemsFiltered)
            {
                if (!_item.IsValid)
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }
    }

    public class ValidatorResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public string ItemName { get; set; }
    }

    public class ValidatorResults
    {
        public bool IsValid;
        public List<ValidatorResult> InvalidItems;

        public ValidatorResults()
        {
            this.InvalidItems = new List<ValidatorResult>();
            this.IsValid = true;
        }
    }

    public enum RequiredOnlyTextType { OnlyLetters, OnlyLettersAndNumbers, OnlyLettersNumbersAndUnderscore, OnlyNumbers }
}