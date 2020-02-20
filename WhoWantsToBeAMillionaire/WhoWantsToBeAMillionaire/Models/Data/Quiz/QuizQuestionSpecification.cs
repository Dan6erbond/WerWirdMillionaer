using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizQuestionSpecification : ISpecification<QuizQuestion>
    {
        private readonly List<int> _categories;
        private readonly List<int> _excludeCategories;

        public QuizQuestionSpecification(List<int> categories = null, List<int> excludeCategories = null)
        {
            _categories = categories;
            _excludeCategories = excludeCategories;
        }

        public bool Specificied(QuizQuestion item)
        {
            if (_categories != null && !_categories.Contains(item.CategoryId))
            {
                return false;
            }

            if (_excludeCategories != null && _excludeCategories.Contains(item.CategoryId))
            {
                return false;
            }

            return true;
        }
    }
}