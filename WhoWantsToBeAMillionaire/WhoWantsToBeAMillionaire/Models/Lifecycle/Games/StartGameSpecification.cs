using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Api.ValidationAttributes;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class StartGameSpecification
    {
        [ListCount(1, ErrorMessage = "At least one category must be selected.")]
        public List<int> Categories { get; set; }
    }
}