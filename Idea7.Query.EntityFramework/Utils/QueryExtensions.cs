using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Idea7.Query.EntityFramework.Utils
{
    internal class QueryExtensions
    {
        public static Expression<Func<TClass, object>> CreateExpression<TClass>(string property)
        {
            var props = property.Split('.');
            var type = typeof(TClass);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            foreach (string prop in props)
            {
                var pi = type.GetProperties().Single(s => s.Name.Equals(prop, StringComparison.CurrentCultureIgnoreCase));

                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            if (expr.Type.GetTypeInfo().IsValueType)
            {
                expr = Expression.Convert(expr, typeof(object));
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(TClass), typeof(object));
            return (Expression<Func<TClass, object>>)Expression.Lambda(delegateType, expr, arg);
        }
    }
}