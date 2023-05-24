using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Models
{
    [Table("ticket")]
    [EnableCors("CorsPolicy")]
    public class Ticket
    {
        [Key,Required]
        public int id { get; set; }
        [Required]
        public string title { get; set; }
        [AllowNull]
        public string? Description { get; set; }
        public string? Summary { get; set; }
        public string? Tags { get; set; }
        [Required]
        public string Status { get; set; }
    
        public string? Type { get; set; }
        
        public string? Priority { get; set; }
  
        public string? Assignee { get; set; }
    
        public string? Color { get; set; }

        // Foreign key for Project
        [Required]
        public int ProjectId { get; set; }

        // Navigation property
        [Required]
        public Project Project { get; set; }
    }
}
