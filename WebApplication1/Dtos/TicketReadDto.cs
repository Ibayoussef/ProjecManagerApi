namespace WebApplication1.Dtos
{
    public class TicketReadDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int ProjectId { get; set; }
    }
}
