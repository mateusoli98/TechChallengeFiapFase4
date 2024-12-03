using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace CreateAPI.Controllers;

[ApiController]
public class BaseController : ControllerBase
{    
    protected async Task<ActionResult> Result<TValue>(Task<ErrorOr<TValue>> resultTask)
    {
        var result = await resultTask;

        if (result.IsError)
        {
            if (result.FirstError.Type == ErrorType.NotFound)
            {
                return NotFound();
            }

            return Error(string.Concat(result.Errors.Select(e => $"{e.Description}, ")), HttpStatusCode.BadRequest);
        }

        return Ok(result.Value);
    }

    protected async Task<ActionResult> Result(Task<Error?> resultTask)
    {
        var result = await resultTask;

        if (result.HasValue)
        {
            if (result.Value.Type == ErrorType.NotFound)
                return NotFound();

            return Error(result.ToString()!, HttpStatusCode.BadRequest);
        }

        return NoContent();
    } 

    protected ActionResult Error(string message, HttpStatusCode statusCode)
    {
        var error = new
        {
            Message = message
        };

        return new ObjectResult(error)
        {
            StatusCode = (int)statusCode
        };
    }
}
