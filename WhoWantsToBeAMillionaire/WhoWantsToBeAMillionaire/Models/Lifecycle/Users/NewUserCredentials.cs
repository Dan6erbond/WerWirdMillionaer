using System.ComponentModel.DataAnnotations;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Users
{
    public class NewUserCredentials
    {
        [Required(ErrorMessage = "A username is required.")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Please enter your original password.")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "A new password is required.")]
        public string OldPassword { get; set; }
    }
}