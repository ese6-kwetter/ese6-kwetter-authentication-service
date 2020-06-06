using System.ComponentModel.DataAnnotations;

namespace UserMicroservice.Models
{
    public class RegisterAppleModel
    {
        [Required] public string Token { get; set; }
    }
}
