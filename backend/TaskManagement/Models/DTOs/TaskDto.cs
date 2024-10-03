namespace TaskManagement.Models.DTOs
{
    public class TaskDto
    {
        public String Title { get; set; } = String.Empty;
        public Int32 StateId { get; set; }
        public String UserId { get; set; } = String.Empty;
    }
}
