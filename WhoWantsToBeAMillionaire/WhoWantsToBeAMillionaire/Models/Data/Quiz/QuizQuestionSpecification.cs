using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizQuestionSpecification : ISpecification<QuizQuestion>
    {
        private readonly List<int> _categories;
        private readonly List<int> _excludeQuestions;

        public QuizQuestionSpecification(List<int> categories = null, List<int> excludeQuestions = null)
        {
            _categories = categories;
            _excludeQuestions = excludeQuestions;
        }

        public bool Specificied(QuizQuestion item)
        {
            if (_categories != null && !_categories.Contains(item.CategoryId))
            {
                return false;
            }

            if (_excludeQuestions != null && _excludeQuestions.Contains(item.QuestionId))
            {
                return false;
            }

            return true;
        }
    }
}