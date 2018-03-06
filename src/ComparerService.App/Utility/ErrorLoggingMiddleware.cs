// <copyright file="ErrorLoggingMiddleware.cs" company="ZoralLabs">
//   Copyright Zoral Limited 2014 all rights reserved.
//   Copyright Zoral Inc. 2014 all rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ComparerService.App.Utility
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var logger = context.RequestServices.GetService<ILogger<ErrorLoggingMiddleware>>();
                logger?.LogError(ex, "Unhadled error");

                throw;
            }
        }
    }
}