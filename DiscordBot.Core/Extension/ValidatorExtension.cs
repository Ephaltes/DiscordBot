using DiscordBot.Core.Validator;
using FluentValidation;

namespace DiscordBot.Core.Extension
{
    public static class ValidatorExtension
    {
        public static IRuleBuilderOptions<T, TProperty> IsValidDate<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new DateValidator<T, TProperty>());
        }
    }
}