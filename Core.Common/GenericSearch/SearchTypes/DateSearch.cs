using System;
using System.Linq.Expressions;
using Core.Common.Enums;

namespace Core.Common.GenericSearch.SearchTypes
{
    public sealed class DateSearch : BaseSearch
    {
        public DateTime? SearchTerm { get; set; }
        
        public DateTime? SearchTerm2 { get; set; }
        
        public EnumDateComparator Comparator { get; set; }
        
        protected override Expression BuildFilterExpression(Expression property)
        {
            var searchExpression1 = GetFilterExpression(property);
            
            Expression searchExpression2 = null;

            if (Comparator == EnumDateComparator.InRange && SearchTerm2 != null)
            {
                searchExpression2 = Expression.LessThanOrEqual(property, Expression.Constant(SearchTerm2.Value));
            }

            if (searchExpression1 == null && searchExpression2 == null)
            {
                return null;
            }

            if (searchExpression1 != null && searchExpression2 != null)
            {
                return Combine(searchExpression1, searchExpression2);
            }
            
            return searchExpression1 ?? searchExpression2;
        }

        private Expression GetFilterExpression(Expression property)
        {
            if (SearchTerm == null)
            {
                return null;
            }
            
            switch (Comparator)
            {
                case EnumDateComparator.Less:
                    return Expression.LessThan(property, Expression.Constant(SearchTerm.Value));
                case EnumDateComparator.LessOrEqual:
                    return Expression.LessThanOrEqual(property, Expression.Constant(SearchTerm.Value));
                case EnumDateComparator.Equal:
                    return Expression.Equal(property, Expression.Constant(SearchTerm.Value));
                case EnumDateComparator.GreaterOrEqual:
                case EnumDateComparator.InRange:
                    return Expression.GreaterThanOrEqual(property, Expression.Constant(SearchTerm.Value));
                case EnumDateComparator.Greater:
                    return Expression.GreaterThan(property, Expression.Constant(SearchTerm.Value));
                default:
                    throw new InvalidOperationException("Comparator not supported.");
            }
        }
    }
}