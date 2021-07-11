using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Common.Enums;

namespace Core.Common.GenericSearch
{
    public sealed class Sort
    {
        public Sort(string property, EnumSortDirection direction)
        {
            Property = property;
            Direction = direction;
        }

        public string Property { get; }

        public EnumSortDirection Direction { get; }

        public IQueryable<T> ApplyToQuery<T>(IQueryable<T> query)
        {
            var parts = Property.Split('.');

            var parameterExpression = Expression.Parameter(typeof(T), "p");

            var property = typeof(T).GetProperty(parts[0]);

            var memberExpression = Expression.MakeMemberAccess(parameterExpression,
                property ?? throw new InvalidOperationException("Property not exist."));

            for (var index = 1; index < parts.Length; index++)
            {
                property = property.PropertyType.GetProperty(parts[index]);

                memberExpression = Expression.MakeMemberAccess(memberExpression,
                    property ?? throw new InvalidOperationException("Property not exist."));
            }

            var methodName = Direction == EnumSortDirection.Ascending ? "OrderBy" : "OrderByDescending";

            var lambdaExpression = Expression.Lambda(memberExpression, parameterExpression);

            Expression expression = Expression.Call(typeof(Queryable), methodName,
                new[] {typeof(T), property.PropertyType}, query.Expression,
                (Expression) Expression.Quote(lambdaExpression));

            return query.Provider.CreateQuery<T>(expression);
        }
    }
}