using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace SRCTech.Common.Tests
{
    public static class NullTestingUtilities
    {
        public static void TestNullParameters(Expression<Action> expression, string parameterToTest)
        {
            Expression revisedBodyExpression;
            if (expression.Body is MethodCallExpression methodCallExpression)
            {
                var target = methodCallExpression.Object;
                var method = methodCallExpression.Method;
                var arguments = methodCallExpression.Arguments
                    .ReplaceArgumentWithNull(method.GetParameters(), parameterToTest);

                revisedBodyExpression = Expression.Call(target, method, arguments);
            }
            else if (expression.Body is NewExpression newExpression)
            {
                var constructor = newExpression.Constructor;
                var arguments = newExpression.Arguments
                    .ReplaceArgumentWithNull(constructor.GetParameters(), parameterToTest);

                revisedBodyExpression = Expression.New(constructor, arguments);
            }
            else
            {
                throw new InvalidOperationException();
            }

            var action = Expression.Lambda<Action>(revisedBodyExpression).Compile();
            var exception = Assert.Throws<ArgumentNullException>(action);
            Assert.Equal(parameterToTest, exception.ParamName);
        }

        private static Expression[] ReplaceArgumentWithNull(
            this IEnumerable<Expression> arguments,
            IEnumerable<ParameterInfo> parameters,
            string parameterToReplace)
        {
            var (parameter, index) = parameters
                .Select((x, i) => (Parameter: x, Index: i))
                .Where(x => x.Parameter.Name.Equals(parameterToReplace))
                .Single();

            var result = arguments.ToArray();
            result[index] = Expression.Constant(null, parameter.ParameterType);
            return result;
        }
    }
}
