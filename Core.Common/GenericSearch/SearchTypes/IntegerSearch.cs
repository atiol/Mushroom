using System;
using System.Linq.Expressions;
using Core.Common.Enums;

namespace Core.Common.GenericSearch.SearchTypes
{
    public sealed class IntegerSearch : BaseSearch
    {
        public int? SearchTerm { get; set; }

        public EnumNumericComparator Comparator { get; set; }

        protected override Expression BuildFilterExpression(Expression property)
        {
            if (SearchTerm == null)
            {
                return null;
            }
            
            switch (Comparator)
            {
                case EnumNumericComparator.Less:
                    return Expression.LessThan(property, Expression.Constant(SearchTerm.Value));
                case EnumNumericComparator.LessOrEqual:
                    return Expression.LessThanOrEqual(property, Expression.Constant(SearchTerm.Value));
                case EnumNumericComparator.Equal:
                    return Expression.Equal(property, Expression.Constant(SearchTerm.Value));
                case EnumNumericComparator.GreaterOrEqual:
                    return Expression.GreaterThanOrEqual(property, Expression.Constant(SearchTerm.Value));
                case EnumNumericComparator.Greater:
                    return Expression.GreaterThan(property, Expression.Constant(SearchTerm.Value));
                default:
                    throw new InvalidOperationException("Comparator not supported.");
            }
        }
    }
}