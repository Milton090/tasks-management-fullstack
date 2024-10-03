namespace TaskManagement.Models
{
    public class ResponseAPI
    {
        public Boolean Success { get; set; }
        public Int32 StatusCode { get; set; }
        public String Message { get; set; } = String.Empty;
        public Object? Data { get; set; }
    }
}
