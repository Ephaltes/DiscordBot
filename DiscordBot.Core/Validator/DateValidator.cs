using System;
using FluentValidation;
using FluentValidation.Validators;

namespace DiscordBot.Core.Validator
{
    public class DateValidator<T, TProperty> : PropertyValidator<T, TProperty>, IDateValidator
    {
        public override string Name => "DateValidator";
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDateValidator : IPropertyValidator
    {
    }
}