using Core.Common.Enums;

namespace Core.Common.Models
{
    public sealed class ApiResponse<T>
    {
        public ApiResponse(ResponseMessage responseMessage, EnumLanguage language, T result)
        {
            Success = responseMessage.Success;
            Message = responseMessage.Message[language];
            Code = responseMessage.Code;
            Result = result;
        }

        public bool Success { get; }

        public string Message { get; }

        public T Result { get; }

        public int Code { get; }
    }
    
    public sealed class ApiResponse
    {
        public ApiResponse(ResponseMessage responseMessage, EnumLanguage language)
        {
            Success = responseMessage.Success;
            Message = responseMessage.Message[language];
            Code = responseMessage.Code;
        }

        public bool Success { get; }

        public string Message { get; }

        public int Code { get; }
    }
}