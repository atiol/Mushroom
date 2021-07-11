using System;
using AutoMapper;
using Core.Common.Enums;
using Core.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Api.Abstracts
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        protected readonly ResponseMessageCommonService ResponseMessageCommonService;
        private readonly IMapper _mapper;

        protected BaseController(IServiceProvider serviceProvider)
        {
            _httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            
            _configuration = serviceProvider.GetService<IConfiguration>();
            
            _mapper = serviceProvider.GetService<IMapper>();
            
            ResponseMessageCommonService = serviceProvider.GetService<ResponseMessageCommonService>();
        }

        protected EnumLanguage GetLanguage()
        {
            var lang = _httpContextAccessor.HttpContext.Request.Headers["X-Lang"].ToString();

            return string.IsNullOrEmpty(lang)
                ? Enum.Parse<EnumLanguage>(_configuration["DefaultLanguage"])
                : Enum.Parse<EnumLanguage>(lang);
        }
        
        protected string GetConfigurationValue(string key)
        {
            return _configuration[key];
        }

        protected IMapper GetMapper()
        {
            return _mapper;
        }
    }
}