using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizAnswerSpecification : ISpecification<QuizAnswer>
    {
        private readonly int? _questionId;

        public QuizAnswerSpecification(int? questionId = null)
        {
            _questionId = questionId;
        }

        public bool Specificied(QuizAnswer item)
        {
            if (_questionId != null && _questionId != item.QuestionId)
            {
                return false;
            }

            return true;
        }
    }
}