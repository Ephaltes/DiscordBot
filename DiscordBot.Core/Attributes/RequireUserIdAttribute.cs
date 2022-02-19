using System.Threading.Tasks;
using DSharpPlus.SlashCommands;

namespace DiscordBot.Core.Attributes;

public class RequireUserIdAttribute : SlashCheckBaseAttribute
{
    private readonly ulong UserId;

    public RequireUserIdAttribute(ulong userId)
    {
        UserId = userId;
    }

    public override async Task<bool> ExecuteChecksAsync(InteractionContext ctx)
    {
        return ctx.User.Id == UserId;
    }
}