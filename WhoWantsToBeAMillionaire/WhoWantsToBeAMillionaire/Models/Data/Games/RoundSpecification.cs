using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class RoundSpecification : ISpecification<Round>
    {
        private readonly int? _gameId;
        private readonly List<int> _answerIds;
        private readonly int? _questionId;
        
        public RoundSpecification(int? gameId = null, List<int> answerIds = null, int? questionId = null)
        {
            _gameId = gameId;
            _answerIds = answerIds;
            _questionId = questionId;
        }

        public bool Specificied(Round item)
        {
            if (_gameId != null && _gameId != item.GameId)
            {
                return false;
            }
            
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