using System;
using System.Linq.Expressions;
using Core.Common.Enums;

namespace Core.Common.GenericSearch.SearchTypes
{
    public sealed class GuidSearch : BaseSearch
    {
        public Guid SearchTerm { get; set; }

        public EnumGuidComparator Comparator { get; set; }

        protected override Expression BuildFilterExpression(Expression property)
        {
            if (SearchTerm == Guid.Empty)
            {
                return null;
            }

            var method = typeof(Guid).GetMethod(EnumGuidComparator.Equal.ToString(), new[] {typeof(Guid)});

            return Expression.Call(property, method ?? throw new InvalidOperationException("Comparator not supported."),
                Expression.Constant(SearchTerm));
        }
    }
}