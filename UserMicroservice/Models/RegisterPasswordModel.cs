using System.ComponentModel.DataAnnotations;

namespace UserMicroservice.Models
{
    public class RegisterPasswordModel
    {
        [Required] public string Username { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
    }
}
