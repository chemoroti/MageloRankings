using MageloRankings.Interface;
using MageloRankings.Models;
using System;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;

namespace MageloRankings.Services
{
    public class InMemoryDatabaseService : IDatabase
    {
        public static List<Character> Data = new List<Character>();
        public static Dictionary<string, List<Character>> Dict_Class = new Dictionary<string, List<Character>>();
        public InMemoryDatabaseService() { }

        public async Task PopulateDatabase(StreamContent stream)
        {
            Data.Clear();
            var allData = await stream.ReadAsStringAsync();
            string[] rows = allData.Split('\n').Skip(1).ToArray();

            foreach (string row in rows)
            {
                if (!string.IsNullOrWhiteSpace(row))
                {
                    Data.Add(new Character(row));
                }
            }
        }

        public List<Character> Search(QueryParams queryParams)
        {
            IQueryable<Character> query = Data.AsQueryable();
            
            foreach (Filter filter in queryParams.Filters)
            {
                Expression<Func<Character, bool>> predicate = CreatePredicate(filter);
                query = query.Where(predicate);
            }

            PropertyInfo? propertyInfo = typeof(Character).GetProperty(queryParams.SortField.ToString());
            if (propertyInfo != null)
            {
                query = query.OrderBy(p => propertyInfo.GetValue(p, null));
            }

            if (queryParams.SortOrder == SortOrderEnum.Descending)
            {
                query = query.Reverse();
            }

            return (queryParams.Take == -1) 
                ? query.ToList() 
                : query.Take(queryParams.Take).ToList();
        }

        private Expression<Func<Character, bool>> CreatePredicate(Filter filter)
        {
            if (string.IsNullOrEmpty(filter.Value))
            {
                throw new ArgumentNullException(nameof(filter.Value));
            }

            try
            {
                Type characterType = typeof(Character);
                ParameterExpression expression = Expression.Parameter(characterType, "expression");
                PropertyInfo? column = characterType.GetProperties().FirstOrDefault(p => p.Name == filter.Field.ToString());

                if (column == null)
                {
                    throw new ArgumentException($"InMemoryDatabaseService::CreatePredicate Invalid field: {filter.Field}");
                }

                Expression body = Expression.Constant(true);
                Func<Expression, Expression, BinaryExpression> func = GetBinaryExpression(filter.Operator);

                if (column?.PropertyType == typeof(int))
                {
                    int asInt = int.Parse(filter.Value.ToString() ?? "");
                    body = func(Expression.PropertyOrField(expression, filter.Field.ToString()), Expression.Constant(asInt));
                }
                else if (column?.PropertyType == typeof(string))
                {
                    body = func(Expression.PropertyOrField(expression, filter.Field.ToString()), Expression.Constant(filter.Value));
                }

                return Expression.Lambda<Func<Character, bool>>(body, expression);
            }
            catch (Exception ex)
            {
                throw new Exception("InMemoryDatabaseService::CreatePredicate Something went wrong", ex);
            }
        }

        public Func<Expression, Expression, BinaryExpression> GetBinaryExpression(OperatorEnum op)
        {
            switch (op)
            {
                case OperatorEnum.Equal:
                    return (a, b) => Expression.Equal(a, b);
                case OperatorEnum.NotEqual:
                    return (a, b) => Expression.NotEqual(a, b);
                case OperatorEnum.LessThan:
                    return (a, b) => Expression.LessThan(a, b);
                case OperatorEnum.LessThanOrEqual:
                    return (a, b) => Expression.LessThanOrEqual(a, b);
                case OperatorEnum.GreaterThan:
                    return (a, b) => Expression.GreaterThan(a, b);
                case OperatorEnum.GreaterThanOrEqual:
                    return (a, b) => Expression.GreaterThanOrEqual(a, b);

                default:
                    throw new NotImplementedException($"GetBinaryExpression::invalid operator: {op}");

            }
        }
    }
}
