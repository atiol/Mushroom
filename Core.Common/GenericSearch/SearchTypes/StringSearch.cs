using System;
using System.Linq.Expressions;
using Core.Common.Enums;

namespace Core.Common.GenericSearch.SearchTypes
{
    public sealed class StringSearch : BaseSearch
    {
        public string SearchTerm { get; set; }

        public EnumStringComparator Comparator { get; set; }

        protected override Expression BuildFilterExpression(Expression property)
        {
            if (SearchTerm == null)
            {
                return null;
            }

            var method = typeof(string).GetMethod(Comparator.ToString(), new[] {typeof(string)});

            return Expression.Call(property,
                method ?? throw new InvalidOperationException("Comparator not supported."),
                Expression.Constant(SearchTerm));
        }
    }
}