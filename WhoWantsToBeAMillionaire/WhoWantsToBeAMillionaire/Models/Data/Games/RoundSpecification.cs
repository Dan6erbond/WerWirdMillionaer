using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class RoundSpecification : ISpecification<Round>
    {
        private readonly List<int> _answerIds;
        private readonly int? _questionId;

        public RoundSpecification(List<int> answerIds = null, int? questionId = null)
        {
            _answerIds = answerIds;
            _questionId = questionId;
        }

        public bool Specificied(Round item)
        {
            if (_answerIds != null && item.AnswerId != null && !_answerIds.Contains(item.AnswerId.Value))
            {
                return false;
            }
            
            if (_questionId != null && _questionId != item.QuestionId)
            {
                return false;
            }

            return true;
        }
    }
}