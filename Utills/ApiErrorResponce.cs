namespace OrderManagement.API.Utills
{
    public class ApiErrorResponce
    {
        public int statusCode {  get; set; }
        public required string Response { get; set; }
        public bool Success { get; set; }

    }
}
