using System.Text.Json;
namespace RealEstateManagement.API.Middleware
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string? StackTrace { get; set; } // Optional, useful in dev mode

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
