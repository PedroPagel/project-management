using System.Linq.Expressions;
using System.Reflection;
using Project.Management.Domain.Entities;

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

        public static Entity UpdateIfDifferent2(this Entity entity, Type sourceProperty, object propertyValue1)
        {
            string name = sourceProperty.Name;

           var valueDestination = sourceProperty.GetProperty(name)?.GetValue(entity, null);

            if (valueDestination != propertyValue1)
            {
                var property = entity.GetType().GetProperty(name);

                property?.SetValue(entity, propertyValue1);
            }

            return entity;
        }
    }
}
