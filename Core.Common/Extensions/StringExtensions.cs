using System.Globalization;
using System.Text;

namespace Core.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToBeautify(this string str)
        {
            var outputStringBuilder = new StringBuilder("");

            if (string.IsNullOrEmpty(str))
            {
                return outputStringBuilder.ToString();
            }

            var split = str.Split(" ");

            foreach (var s in split)
            {
                if (string.IsNullOrEmpty(s)) continue;
                
                var charArr = s.ToCharArray();

                for (var i = 0; i < charArr.Length; i++)
                {
                    if (i == 0)
                    {
                        outputStringBuilder.Append(charArr[i].ToString().Replace('i', 'İ').Replace('ı', 'I')
                            .ToUpper(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        outputStringBuilder.Append(charArr[i].ToString().Replace('İ', 'i').Replace('I', 'ı')
                            .ToLower(CultureInfo.InvariantCulture));
                    }
                }

                outputStringBuilder.Append(" ");
            }

            outputStringBuilder.Remove(outputStringBuilder.Length - 1, 1);

            return outputStringBuilder.ToString();
        }
        
        public static string ToNormalize(this string str)
        {
            return str.Replace('i', 'İ').Replace('ı', 'I').Normalize().ToUpperInvariant().RemoveWhiteSpace();
        }
        
        public static string RemoveWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str) ? "" : str.Replace(" ", "");
        }
    }
}