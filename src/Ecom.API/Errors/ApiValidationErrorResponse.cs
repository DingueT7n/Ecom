namespace Ecom.API.Errors
{
    public class ApiValidationErrorResponse : BaseCommuneResponse
    {
        public IEnumerable<string> Errors  { get; set; }
        public ApiValidationErrorResponse() : base(400)
        {
        }
    }
}
