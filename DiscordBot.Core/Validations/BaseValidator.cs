using FluentValidation;

namespace DiscordBot.Core.Validations
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public BaseValidator()
        {
            CascadeMode = CascadeMode.Stop;
        }
    }
}