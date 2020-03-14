using System.Collections.Generic;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Games;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class GameSpecification : ISpecification<Game>
    {
        private readonly int? _userId;
        
        public GameSpecification(int? userId = null)
        {
            _userId = userId;
        }

        public bool Specificied(Game item)
        {
            if (_userId != null && _userId != item.UserId)
            {
                return false;
            }

            return true;
        }
    }
}