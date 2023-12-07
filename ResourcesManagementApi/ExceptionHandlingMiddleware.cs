using ResourcesManagementApi.Domain.Exceptions;
using System.Data;
using System.Net;

namespace ResourcesManagementApi
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                switch (exception)
                {
                    case EntityNotFoundException:
                    case BusinessRuleValidationException:
                    case DBConcurrencyException:
                        var statusCode = GetStatusCode(exception);

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)statusCode;

                        await context.Response.WriteAsJsonAsync(exception.Message);
                        break;
                    default:
                        throw;
                }
            }
        }

        private static HttpStatusCode GetStatusCode(Exception exception) 
        {
            switch (exception)
            {
                case EntityNotFoundException entityNotFoundException:
                    return HttpStatusCode.NotFound;
                case BusinessRuleValidationException businessRuleValidationException:
                    return HttpStatusCode.Forbidden;
                case DBConcurrencyException dbConcurrencyException:
                    return HttpStatusCode.Conflict;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}
