using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    [EnableCors("CorsPolicy")]
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
