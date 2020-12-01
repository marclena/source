using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Web.Mvc;

namespace Vueling.XXX.WebUI
{
    public static class TelerikQueryableExtensions
    {
        public static IQueryable<T> ApplyFilters<T>(GridCommand command, IQueryable<T> data)
        {
            var filters = GetFilterDescriptors(command);

            return ApplyWhereClause<T>(data, filters);
        }

        public static List<FilterDescriptor> GetFilterDescriptors(GridCommand command)
        {
            var filters = new List<FilterDescriptor>();

            if (!command.FilterDescriptors.Any()) { return filters; }

            foreach (IFilterDescriptor filter in command.FilterDescriptors)
            {
                filters.AddRange(GetFiltersRecursively(filter));
            }

            return filters;
        }

        private static List<FilterDescriptor> GetFiltersRecursively(IFilterDescriptor filter)
        {
            var filtersNested = new List<FilterDescriptor>();

            if (filter is CompositeFilterDescriptor)
            {
                foreach (var childFilter in ((CompositeFilterDescriptor)filter).FilterDescriptors)
                {
                    var nestedFilter = GetFiltersRecursively(childFilter);
                    filtersNested.AddRange(nestedFilter);
                }
            }
            else
            {
                filtersNested.Add((FilterDescriptor)filter);
            }

            return filtersNested;
        }

        private static IQueryable<T> ApplyWhereClause<T>(IQueryable<T> data, List<FilterDescriptor> filters)
        {
            Expression<Func<T, bool>> expression = null;

            foreach (FilterDescriptor filter in filters)
            {
                expression = ConvertToExpression<T>(
                        filter.Member.ToString(),
                        filter.Operator.ToString(),
                        filter.Value
                        );
                
                if (expression != null) { data = data.Where(expression); }
            }
            return data;
        }

        private static Expression<Func<T, bool>> ConvertToExpression<T>(string propName, string opr, object value)
        {
            ParameterExpression parameterExp = Expression.Parameter(typeof(T));
            var propertyExp = Expression.Property(parameterExp, propName);

            var objectValueType = value.GetType();

            if (objectValueType != propertyExp.Type)
            {
                objectValueType = propertyExp.Type;
                value = Convert.ChangeType(value, propertyExp.Type);
            }

            var constantValue = Expression.Constant(value, objectValueType);
            Expression expressionCall = null;

            switch (opr)
            {
                case "Contains":
                case "EndsWith":
                case "StartsWith":
                    expressionCall = GetExpressionCallForString(value, propertyExp, constantValue, opr);
                    break;

                case "DoesNotContain":
                    expressionCall = Expression.Not(GetExpressionCallForString(value, propertyExp, constantValue, "Contains"));
                    break;

                //case "IsContainedIn":

                case "IsEqualTo":
                    expressionCall = Expression.Equal(propertyExp, constantValue);
                    break;
                case "IsGreaterThan":
                    expressionCall = Expression.GreaterThan(propertyExp, constantValue);
                    break;
                case "IsGreaterThanOrEqualTo":
                    expressionCall = Expression.GreaterThanOrEqual(propertyExp, constantValue);
                    break;
                case "IsLessThan":
                    expressionCall = Expression.LessThan(propertyExp, constantValue);
                    break;
                case "IsLessThanOrEqualTo":
                    expressionCall = Expression.LessThanOrEqual(propertyExp, constantValue);
                    break;
                case "IsNotEqualTo":
                    expressionCall = Expression.NotEqual(propertyExp, constantValue);
                    break;

                default:
                    throw new NotImplementedException(string.Format("Method for operation {0} is not implemented.", opr));
            }

            return Expression.Lambda<Func<T, bool>>(expressionCall, parameterExp);
        }

        private static Expression GetExpressionCallForString(object value, MemberExpression propertyExp, ConstantExpression constantValue, string filterOperator)
        {
            if (value.GetType() != typeof(string)) { throw new InvalidOperationException(string.Format("Operator {0} is only valid for data type string.", filterOperator)); }

            var methodInfo = value.GetType().GetMethod(filterOperator, new[] { typeof(string) });
            return Expression.Call(propertyExp, methodInfo, constantValue);
        }

    }
}