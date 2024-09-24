﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OgmentoAPI.Domain.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OgmentoAPI.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;
        public record ExceptionResponse(HttpStatusCode StatusCode, string Description);
        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, $"An unexpected error occurred. Request Details: {context.Request.Path}, QueryString: {context.Request.QueryString.ToString()}, " +
                $"Method: {context.Request.Method}, User: {context.User?.FindFirst(c=>c.Type == CustomClaimTypes.UserName)?.Value ?? "Anonymous"}");

            ExceptionResponse response = exception switch
            {
                ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
                KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found."),
                UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

