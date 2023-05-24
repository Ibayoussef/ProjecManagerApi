using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Models
{
    [EnableCors("CorsPolicy")]
    [Table("project")]
   
    public class Project
    {
        [Key, Required]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [AllowNull]
        public string description { get; set; }
        [Required]
        // Foreign key for User
        public string UserId { get; set; }

        // Navigation property
        public IdentityUser User { get; set; }
        // Navigation property
        public ICollection<Ticket> Tickets { get; set; }
    }
}
