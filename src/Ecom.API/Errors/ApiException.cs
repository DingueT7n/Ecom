using System.ComponentModel.DataAnnotations;

namespace Ecom.API.Errors
{
    public class ApiException : BaseCommuneResponse
    {
    
        public string details { get; set; }

        public ApiException(int statusCode, string message = null,string d=null) : base(statusCode, message)
        {
            details = d;
        }

    }
}
