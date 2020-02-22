using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WhoWantsToBeAMillionaire.Models.Api.ValidationAttributes
{
    public class ListCount : ValidationAttribute
    {
        private readonly int _minCount;
        private readonly int? _maxCount;
        
        public ListCount(int minCount)
        {
            _minCount = minCount;
        }

        public ListCount(int minCount, int maxCount)
        {
            _minCount = minCount;
            _maxCount = maxCount;
        }

        public override bool IsValid(object value)
        {
            var list = (List<object>) value;
            var count = list.Count;

            if (count < _minCount)
            {
                return false;
            }

            if (_maxCount != null && count > _maxCount)
            {
                return false;
            }

            return true;
        }
    }
}