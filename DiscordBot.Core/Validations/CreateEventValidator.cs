using DiscordBot.Core.Dtos;
using FluentValidation;

namespace DiscordBot.Core.Validations
{
    public class CreateEventValidator : BaseValidator<CreateEventDto>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Time).NotEmpty();
            RuleFor(x => x.EventChannel).NotEmpty();

            RuleFor(x => x.Reminder).NotNull().WithMessage("Wrong Reminder Format");
        }
    }
}