using System.Net;

namespace AttendanceApi.Utils
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public AppException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
