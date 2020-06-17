using System.ComponentModel.DataAnnotations;

namespace UserMicroservice.Models
{
    public class LoginAppleModel
    {
        [Required] public string Token { get; set; }
    }
}
