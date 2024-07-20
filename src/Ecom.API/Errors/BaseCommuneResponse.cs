

namespace Ecom.API.Errors
{
    public class BaseCommuneResponse
    {
        public BaseCommuneResponse(int statusCode, string message=null)
        {
            StatusCode = statusCode;
            Message = message ?? DefaultMessageForStatusCode(statusCode);
        }

        private string DefaultMessageForStatusCode(int statusCode)
        => statusCode switch
            {

                400 => "Bad Request",
                401 => "Not Authorized",
                404 => "Resource Not Found",
                500 => "Server Error",
                _ => null
            };
        
        

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
