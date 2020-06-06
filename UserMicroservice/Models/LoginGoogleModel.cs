using System.ComponentModel.DataAnnotations;

namespace UserMicroservice.Models
{
    public class LoginGoogleModel
    {
        [Required] public string Token { get; set; }
    }
}
