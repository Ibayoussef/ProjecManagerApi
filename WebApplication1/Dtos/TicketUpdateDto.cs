namespace WebApplication1.Dtos
{
    public class TicketUpdateDto
    {
        public string? title { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? Assignee { get; set; }
        public string? Priority { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
    }
}
