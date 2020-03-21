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

            if (x.Points == y.Points)
            {
                if (x.WeightedPoints > y.WeightedPoints)
                {
                    return -1;
                }

                if (x.WeightedPoints < y.WeightedPoints)
                {
                    return 1;
                }

                return 0;
            }

            return x.Points > y.Points ? -1 : 1;
        }
    }
}