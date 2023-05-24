using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

[EnableCors("CorsPolicy")]
public class ProjectCreateDto
{
    [Required]
    public string name { get; set; }

    public string description { get; set; }
}