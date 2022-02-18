using System.Threading.Tasks;
using DSharpPlus.SlashCommands;

namespace DiscordBot.Core.Attributes;

public class RequireUserIdAttribute : SlashCheckBaseAttribute
{
    public ulong UserId;

    public RequireUserIdAttribute(ulong userId)
    {
        UserId = userId;
    }

    public override async Task<bool> ExecuteChecksAsync(InteractionContext ctx)
    {
        if (ctx.User.Id == UserId)
            return true;

        return false;
    }
}