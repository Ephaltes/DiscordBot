using DiscordBot.Core.Dtos;
using DSharpPlus;
using FluentValidation;

namespace DiscordBot.Core.Validations
{
    public class AddUploadOnlyChannelValidator : BaseValidator<AddUploadOnlyChannelDto>
    {
        public AddUploadOnlyChannelValidator()
        {
            RuleFor(x => x.UploadChannel).NotNull();
            RuleFor(x => x.PostToChannel).NotNull();

            RuleFor(x => x.UploadChannel.Type).Must(x => x == ChannelType.Text)
                .WithMessage("{PropertyName} is not a TextChannel");

            RuleFor(x => x.PostToChannel.Type).Must(x => x == ChannelType.Text)
                .WithMessage("{PropertyName} is not a TextChannel");
        }
    }
}