﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Presentation.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger("ExceptionMiddleware");
#pragma warning disable CA2254 // Template should be a static expression
                logger.LogError($"Something went wrong: {ex.Message} - {ex.StackTrace}");
#pragma warning disable CA2254 // Template should be a static expression
                await HandleExceptionAsync(httpContext);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var errorCode = (int)HttpStatusCode.InternalServerError;
            var message = "Internal Server Error.";

            return context.Response.WriteAsync(JsonConvert.SerializeObject(
                new
                {
                    errorCode,
                    message
                }));
        }
    }
}