using System.Collections.Generic;
using Core.Common.Enums;

namespace Core.Common.Models
{
    public sealed class ResponseMessage
    {
        public int Code { get; set; }

        public bool Success { get; set; }

        public IDictionary<EnumLanguage, string> Message { get; set; }
    }
}