using System.Collections.Generic;
using System.Linq;

namespace Core.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static string JoinAsString(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }
        
        public static bool IsNotNullOrEmpty(this IEnumerable<string> source)
        {
            return source != null && source.Any();
        }
    }
}