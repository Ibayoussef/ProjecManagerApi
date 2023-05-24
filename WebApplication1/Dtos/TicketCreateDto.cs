using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

[EnableCors("CorsPolicy")]
public class TicketCreateDto
{
    [Required]
    public string title { get; set; }
    [AllowNull]
    public string? Description { get; set; }
    public string? Summary { get; set; }
  
    public string? Status { get; set; }

    public string? Type { get; set; }

    public string? Priority { get; set; }
    public string? Tags { get; set; }

    public string? Assignee { get; set; }

    public string? Color { get; set; }

    [Required]
    public int ProjectId { get; set; }
}