using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Core.Common.Enums;
using Core.Common.Exceptions;
using Core.Common.Models;
using Core.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.MiddleWares
{
    internal sealed class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ResponseMessageCommonService responseMessageCommonService,
            IConfiguration configuration, ILogger<ExceptionMiddleWare> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex, responseMessageCommonService, configuration, logger);
            }
        }

        private static Task HandleException(HttpContext context, Exception ex,
            ResponseMessageCommonService responseMessageCommonService, IConfiguration configuration,
            ILogger logger)
        {
            var langHeader = context.Request.Headers["X-Lang"].ToString();

            var lang = string.IsNullOrEmpty(langHeader)
                ? Enum.Parse<EnumLanguage>(configuration["DefaultLanguage"])
                : Enum.Parse<EnumLanguage>(langHeader);

            var errorId = Guid.NewGuid();

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            string result;

            if (ex is ResponseException responseException)
            {
                result = JsonSerializer.Serialize(
                    new ApiResponse<Guid>(responseException.ResponseMessage, lang, errorId), options);

                logger.Log(LogLevel.Warning, responseException.ResponseMessage.Code, ex, result);
            }
            else
            {
                result = JsonSerializer.Serialize(
                    new ApiResponse<Guid>(responseMessageCommonService.Unknown, lang, errorId), options);

                logger.Log(LogLevel.Error, responseMessageCommonService.Unknown.Code, ex, result);
            }

            context.Response.ContentType = "application/json; charset=utf-8";

            return context.Response.WriteAsync(result);
        }
    }
}