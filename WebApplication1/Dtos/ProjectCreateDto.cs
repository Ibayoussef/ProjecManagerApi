using System.ComponentModel.DataAnnotations;

public class ProjectCreateDto
{
    [Required]
    public string name { get; set; }

    public string description { get; set; }
}