namespace Talabat.APIs.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public IEnumerable<String> Errors { get; set; } = new List<String>();

        public ApiValidationErrorResponse() : base(400)
        {

        }
    }
}
