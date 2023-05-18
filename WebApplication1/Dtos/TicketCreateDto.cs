using System.ComponentModel.DataAnnotations;

public class TicketCreateDto
{
    [Required]
    public string title { get; set; }

    public string Description { get; set; }

    [Required]
    public int ProjectId { get; set; }
}