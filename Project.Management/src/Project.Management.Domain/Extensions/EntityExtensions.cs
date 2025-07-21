using System.Linq.Expressions;
using System.Reflection;

namespace Project.Management.Domain.Extensions
{
    public static class EntityExtensions
    {
        public static TEntity UpdateIfDifferent<TEntity, TProperty>(
            this TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression, TProperty newValue)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            ArgumentNullException.ThrowIfNull(propertyExpression);

            var memberExpression = propertyExpression.Body as MemberExpression ?? throw new ArgumentException("The expression must be a member expression.", nameof(propertyExpression));
            var propertyInfo = memberExpression.Member as PropertyInfo ?? throw new ArgumentException("The member accessed by the expression is not a property.", nameof(propertyExpression));
            
            var currentValue = propertyInfo.GetValue(entity);

            if (!Equals(currentValue, newValue))
            {
                propertyInfo.SetValue(entity, newValue);
            }

            return entity;
        }
    }
}
