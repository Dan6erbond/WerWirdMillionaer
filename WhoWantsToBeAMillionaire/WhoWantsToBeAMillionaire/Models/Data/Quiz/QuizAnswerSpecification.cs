using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Quiz
{
    public class QuizAnswerSpecification : ISpecification<QuizAnswer>
    {
        private readonly int? _answerId;
        private readonly int? _questionId;
        private readonly bool? _correct;

        public QuizAnswerSpecification(int? answerId = null, int? questionId = null, bool? correct = null)
        {
            _answerId = answerId;
            _questionId = questionId;
            _correct = correct;
        }

        public bool Specificied(QuizAnswer item)
        {
            if (_answerId != null && _answerId != item.AnswerId)
            {
                return false;
            }
            
            if (_questionId != null && _questionId != item.QuestionId)
            {
                return false;
            }

            if (_correct != null && _correct != item.Correct)
            {
                return false;
            }

            return true;
        }
    }
}