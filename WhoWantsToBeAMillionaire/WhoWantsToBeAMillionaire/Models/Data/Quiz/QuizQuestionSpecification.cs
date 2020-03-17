using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizQuestionSpecification : ISpecification<QuizQuestion>
    {

        private readonly int? _questionId;
        private readonly int? _categoryId;
        private readonly List<int> _categories;
        private readonly List<int> _excludeQuestions;

        public QuizQuestionSpecification(int? questionId = null, int? categoryId = null, List<int> categories = null, List<int> excludeQuestions = null)
        {
            _questionId = questionId;
            _categoryId = categoryId;
            _categories = categories;
            _excludeQuestions = excludeQuestions;
        }

        public bool Specificied(QuizQuestion item)
        {
            if (_questionId != null && _questionId != item.QuestionId)
            {
                return false;
            }
            
            if (_categoryId != null && _categoryId != item.CategoryId)
            {
                return false;
            }
            
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