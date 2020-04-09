using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Views
{
    public class UserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string TokenId { get; set; }
    }
}
