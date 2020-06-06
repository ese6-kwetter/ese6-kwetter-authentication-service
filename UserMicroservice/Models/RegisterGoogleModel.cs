using System.ComponentModel.DataAnnotations;

namespace UserMicroservice.Models
{
    public class RegisterGoogleModel
    {
        [Required] public string Token { get; set; }
    }
}
