using System;
using Core.Common.Models;

namespace Core.Common.Exceptions
{
    public sealed class ResponseException : Exception
    {
        public ResponseMessage ResponseMessage { get; }
        
        public ResponseException(ResponseMessage responseMessage)
        {
            ResponseMessage = responseMessage;
        }
    }
}