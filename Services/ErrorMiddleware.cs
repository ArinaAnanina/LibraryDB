using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LibraryDB.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LibraryDB.Services
{
    public class ErrorObject
    {
        public ErrorObject(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public int code { get; set; }
        public string message { get; set; }
    }
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception exception)
            {
                await ErrorEventHandler(context, exception);
            }
        }

        private Task ErrorEventHandler(HttpContext context, Exception exception)
        {
            HttpResponse response = context.Response;

            switch (exception)
            {
                case NotFoundException e:
                    return SendError(response, HttpStatusCode.NotFound, e.Message);
                case BadRequestException e:
                    return SendError(response, HttpStatusCode.BadRequest, e.Message);
                case ServerErrorException e:
                    return SendError(response, HttpStatusCode.InternalServerError, e.Message);
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return SendError(response, HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        private Task SendError(HttpResponse response, HttpStatusCode code, string message)
        {
            response.ContentType = "application/json";
            response.StatusCode = (int)code;

            return response.WriteAsync(JsonSerializer.Serialize(new ErrorObject((int)code, message)));
        }
    }
}
