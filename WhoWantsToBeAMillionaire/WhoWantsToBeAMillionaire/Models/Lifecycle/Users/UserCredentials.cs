using System.ComponentModel.DataAnnotations;

namespace WhoWantsToBeAMillionaire.Models.Lifecycle.Users
{
    public class UserCredentials
    {
        [Required(ErrorMessage = "A username is required.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Usernames must be between 4 and 30 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])((?=.*[0-9])|(?=.*[!@#\$%\^&\*]))(?=.{8,})",
            ErrorMessage =
                "Passwords must be at least 8 characters long, contain an upper- and lower-case letter and a number or special character.")]
        public string Password { get; set; }
    }
}