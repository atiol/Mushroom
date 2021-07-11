using Core.Common.Enums;

namespace Core.Common.GenericSearch
{
    public sealed class Filter
    {
        public Filter(EnumSearchType searchType, string property, string comparator, string value)
        {
            SearchType = searchType;
            Property = property;
            Comparator = comparator;
            Value = value;
        }

        public EnumSearchType SearchType { get; }
        
        public string Property { get; }

        public string Comparator { get; }

        public string Value { get; }
    }
}