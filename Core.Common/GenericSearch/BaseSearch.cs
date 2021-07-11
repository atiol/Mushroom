using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Common.GenericSearch
{
    public abstract class BaseSearch
    {
        public string Property { get; set; }
        
        protected abstract Expression BuildFilterExpression(Expression property);

        public IQueryable<T> ApplyToQuery<T>(IQueryable<T> query)
        {
            var parts = Property.Split('.');

            var parameter = Expression.Parameter(typeof(T), "p");

            var filterExpression = BuildFilterExpressionWithNullChecks(null, parameter, null, parts);

            if (filterExpression == null)
            {
                return query;
            }

            var predicate = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);

            return query.Where(predicate);
        }

        private static bool IsNullableType(Type propertyType)
        {
            return propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static bool IsCollectionType(Type propertyType)
        {
            return propertyType.IsGenericType && typeof(IEnumerable<>)
                       .MakeGenericType(propertyType.GetGenericArguments()).IsAssignableFrom(propertyType);
        }

        protected static Expression Combine(Expression first, Expression second)
        {
            return first == null ? second : Expression.AndAlso(first, second);
        }
        
        private static Expression ApplySearchExpressionToCollection(ParameterExpression parameter, Expression property, Expression searchExpression)
        {
            if (searchExpression == null) return null;
            
            var asQueryable = typeof(Queryable).GetMethods()
                .Where(m => m.Name == "AsQueryable")
                .Single(m => m.IsGenericMethod)
                .MakeGenericMethod(property.Type.GetGenericArguments());

            var anyMethod = typeof(Queryable).GetMethods()
                .Where(m => m.Name == "Any")
                .Single(m => m.GetParameters().Length == 2)
                .MakeGenericMethod(property.Type.GetGenericArguments());

            searchExpression = Expression.Call(
                null,
                anyMethod,
                Expression.Call(null, asQueryable, property),
                Expression.Lambda(searchExpression, parameter));

            return searchExpression;
        }

        private Expression BuildFilterExpressionWithNullChecks(Expression filterExpression,
            ParameterExpression parameter, Expression property, IReadOnlyList<string> remainingPropertyParts)
        {
            property = Expression.Property(property ?? parameter, remainingPropertyParts[0]);

            if (remainingPropertyParts.Count == 1)
            {
                if (!property.Type.IsValueType || IsNullableType(property.Type))
                {
                    var nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));
                    filterExpression = Combine(filterExpression, nullCheckExpression);
                }

                if (IsNullableType(property.Type))
                {
                    property = Expression.Property(property, "Value");
                }

                Expression searchExpression;

                if (IsCollectionType(property.Type))
                {
                    parameter = Expression.Parameter(property.Type.GetGenericArguments().First());

                    searchExpression = ApplySearchExpressionToCollection(
                        parameter,
                        property,
                        BuildFilterExpression(parameter));
                }
                else
                {
                    searchExpression = BuildFilterExpression(property);
                }

                return searchExpression == null ? null : Combine(filterExpression, searchExpression);
            }
            else
            {
                var nullCheckExpression = Expression.NotEqual(property, Expression.Constant(null));
                
                filterExpression = Combine(filterExpression, nullCheckExpression);

                if (!IsCollectionType(property.Type))
                {
                    return BuildFilterExpressionWithNullChecks(filterExpression, parameter, property,
                        remainingPropertyParts.Skip(1).ToArray());
                }
                
                parameter = Expression.Parameter(property.Type.GetGenericArguments().First());
                    
                var searchExpression = BuildFilterExpressionWithNullChecks(
                    null,
                    parameter,
                    null,
                    remainingPropertyParts.Skip(1).ToArray());

                if (searchExpression == null)
                {
                    return null;
                }

                searchExpression = ApplySearchExpressionToCollection(
                    parameter,
                    property,
                    searchExpression);

                return Combine(filterExpression, searchExpression);
            }
        }
    }
}