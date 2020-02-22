using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WhoWantsToBeAMillionaire.Models.Api.ValidationAttributes
{
    public class ListCount : ValidationAttribute
    {
        public int MinCount { get; }
        public int? MaxCount { get; set; }

        public ListCount(int minCount, int? maxCount = null)
        {
            MinCount = minCount;
            MaxCount = maxCount;
        }

        public string GetErrorMessage() => $"The list must contain between {MinCount} and {MaxCount} items.";

        public override bool IsValid(object value)
        {
            var list = (List<object>) value;
            var count = list.Count;

            if (count < MinCount)
            {
                return false;
            }

            if (MaxCount != null && count > MaxCount)
            {
                return false;
            }

            return true;
        }
    }
}