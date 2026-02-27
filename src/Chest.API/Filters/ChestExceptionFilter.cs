using Chest.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chest.API.Filters;

public class ChestExceptionFilter: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationErrorsException ex)
        {
            context.HttpContext.Response.StatusCode = 400; // Bad Request
            context.Result = new BadRequestObjectResult(new { Errors = ex.ErrorMessages });
        }
        else
        {
            // Erro desconhecido (ex: banco caiu)
            context.HttpContext.Response.StatusCode = 500;
            context.Result = new ObjectResult(new { Error = "Erro interno no servidor." });
        }
        context.ExceptionHandled = true;
        
        if (context.Exception is UnauthorizedAccessException)
        {
            context.Result = new ObjectResult(new { message = context.Exception.Message })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            context.ExceptionHandled = true;
        }
    }
}