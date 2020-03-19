using System.Collections.Generic;

namespace WhoWantsToBeAMillionaire.Models.Data.Games
{
    public class GameDateComparer : IComparer<Game>
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
            
            if (x.Start < y.Start)
            {
                return -1;
            }

            return x.Start > y.Start ? 1 : 0;
        }
    }
}