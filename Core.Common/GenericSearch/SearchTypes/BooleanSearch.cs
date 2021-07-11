using System;
using System.Linq.Expressions;
using Core.Common.Enums;

namespace Core.Common.GenericSearch.SearchTypes
{
    public sealed class BooleanSearch : BaseSearch
    {
        public bool? SearchTerm { get; set; }

        public EnumBooleanComparator Comparator { get; set; }

        protected override Expression BuildFilterExpression(Expression property)
        {
            if (SearchTerm == null)
            {
                return null;
            }

            if (Comparator == EnumBooleanComparator.Equal)
            {
                return Expression.Equal(property, Expression.Convert(Expression.Constant(SearchTerm.Value), typeof (bool)));
            }
            
            throw new InvalidOperationException("Comparator not supported.");
        }
    }
}