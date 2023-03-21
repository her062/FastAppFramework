using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace FastAppFramework.Wpf.Extensions
{
    public static class ReactivePropertyExtensions
    {
        public static ReactiveProperty<T> SetValidateAttribute<T>(this ReactiveProperty<T> self, Expression<Func<ReactiveProperty<T>?>> selfSelector, IReadOnlyReactiveProperty<bool> isValidationActive)
        {
            var memberExpression = (MemberExpression)selfSelector.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            var display = propertyInfo.GetCustomAttribute<DisplayAttribute>();
            var attrs = propertyInfo.GetCustomAttributes<ValidationAttribute>().ToArray();
            var context = new ValidationContext(self)
            {
                DisplayName = display?.GetName() ?? propertyInfo.Name,
                MemberName = nameof(self.Value),
            };

            if (attrs.Length != 0)
            {
                self.SetValidateNotifyError(v => {
                    if (!isValidationActive.Value)
                        return null;

                    var validationResults = new List<ValidationResult>();
#pragma warning disable CS8604
                    if (Validator.TryValidateValue(v, context, validationResults, attrs))
                        return null;
#pragma warning restore CS8604

                    return validationResults[0].ErrorMessage;
                });
                isValidationActive.Subscribe(v => self.ForceValidate());
            }
            return self;
        }
    }
}