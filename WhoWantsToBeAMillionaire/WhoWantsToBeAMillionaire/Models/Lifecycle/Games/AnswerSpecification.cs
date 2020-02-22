using System.ComponentModel.DataAnnotations;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Games
{
    public class AnswerSpecification
    {
        [Required(ErrorMessage = "A question ID is required.")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "A answer ID is required.")]
        public int AnswerId { get; set; }
    }
}