
using Domain_Layer.Respones;
using FluentValidation;
using Makeup_Web.Exceptions;

namespace Makeup_Web.Middlewares
{
    public class GlobalExceptionHandler : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
                await next(context);

            }
            catch (ValidationException ex)
			{

                var errorMessages = ex.Errors.Select(e => e.ErrorMessage).ToList();
                var messageString = string.Join(" | ", errorMessages);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var respone= RequestRespones<string>.Fail(messageString,400);

                await context.Response.WriteAsJsonAsync(respone);
            }
            catch(BusinessException ex) 
            {
                context.Response.StatusCode=StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var  response = RequestRespones<string>.Fail(ex.Message, 400);

                await context.Response.WriteAsJsonAsync(response);
            }

            catch(Exception ex) 
            {
                logger.LogError(ex, "System Error");

                context.Response.Redirect($"/Home/Error?message=Something went wrong&statusCode=500");
                return; 
            }
        }
    }
}
