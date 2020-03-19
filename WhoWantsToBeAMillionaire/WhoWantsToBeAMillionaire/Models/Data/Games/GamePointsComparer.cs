using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class GamePointsComparer : IComparer<Game>
    {
        public int Compare(Game x, Game y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }

                return -1;
            }

            if (y == null)
            {
                return 1;
            }
            
            if (x.Points < y.Points)
            {
                return 1;
            }

            return x.Points > y.Points ? -1 : 0;
        }
    }
}