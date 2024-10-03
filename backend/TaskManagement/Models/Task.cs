namespace TaskManagement.Models
{
    public class Task
    {
        public Int32 Id { get; set; }
        public String Title { get; set; } = String.Empty;
        public Int32 StateId { get; set; }
        public String StateName { get; set; } = String.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public String UserId { get; set; } = String.Empty;
    }
}
