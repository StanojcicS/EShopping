using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Common.Exceptions.CustomExceptions;
using Common.Exceptions.CustomExceptions.ShopManagementExceptions;

namespace Common.Exceptions
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case InvalidCredentialsException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case InvalidUsernameException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound; 
                        break;
                    case UserAlreadyExistsException e:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedProductUpdateException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case RemovedProductException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
