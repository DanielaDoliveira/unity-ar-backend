using Chest.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chest.API.Filters;

public class ChestExceptionFilter: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
         if (context.Exception is ValidationErrorsException ex)
            context.Result = new BadRequestObjectResult(new { Errors = ex.ErrorMessages });
         
        else if (context.Exception is UnauthorizedAccessException authEx)
        {
            context.Result = new ObjectResult(new { message = authEx.Message })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
        // No ChestExceptionFilter.cs, mude o 'else' (Caso 3):
        else
        {
            context.Result = new ObjectResult(new {
                Message = "Ocorreu um erro inesperado no navio!",
                Detail = context.Exception.Message, // <-- ADICIONE ISSO
                StackTrace = context.Exception.StackTrace // <-- E ISSO
            }) { StatusCode = 500 };
        }

        context.ExceptionHandled = true;
    }
}