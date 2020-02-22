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
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])((?=.*[0-9])|(?=.*[!@#\$%\^&\*]))(?=.{8,})",
            ErrorMessage =
                "Passwords must be at least 8 characters long, contain an upper- and lower-case letter and a number or special character.")]
        public string OldPassword { get; set; }
    }
}