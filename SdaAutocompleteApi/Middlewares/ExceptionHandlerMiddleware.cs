using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SdaAutocompleteApi.Middlewares
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		private readonly ILogger<ExceptionHandlerMiddleware> _logger;

		public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			_logger.LogError(exception, exception.StackTrace);
			var response = context.Response;
			const int statusCode = (int)HttpStatusCode.InternalServerError;
			response.ContentType = "application/json";
			response.StatusCode = statusCode;
			await response.WriteAsync(JsonConvert.SerializeObject(exception.Message));
		}
	}
}