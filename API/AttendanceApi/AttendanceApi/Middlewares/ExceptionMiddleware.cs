//using AttendanceApi.Utils;
//using System.Text.Json;

//namespace AttendanceApi.Middlewares
//{
//    public class ExceptionMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public ExceptionMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (AppException ex)
//            {
//                context.Response.StatusCode = (int)ex.StatusCode;
//                context.Response.ContentType = "application/json";

//                var response = new
//                {
//                    message = ex.Message
//                };

//                await context.Response.WriteAsync(
//                    JsonSerializer.Serialize(response)
//                );
//            }
//        }
//    }
//}
using System.Net;
using System.Text.Json;
using AttendanceApi.Utils;

namespace AttendanceApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                await WriteErrorResponse(
                    context,
                    (int)ex.StatusCode,
                    ex.StatusCode.ToString(),
                    ex.Message
                );
            }
            catch (Exception)
            {
                // Log the exception here (ILogger) – do NOT expose it
                await WriteErrorResponse(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "INTERNAL_SERVER_ERROR",
                    "Something went wrong. Please try again."
                );
            }
        }

        private static async Task WriteErrorResponse(
            HttpContext context,
            int statusCode,
            string errorCode,
            string message
        )
        {
            if (context.Response.HasStarted)
                return;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new { error = new { code = errorCode, message = message } };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
