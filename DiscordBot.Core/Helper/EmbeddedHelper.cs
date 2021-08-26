using DSharpPlus.Entities;

namespace DiscordBot.Core.Helper
{
    public static class EmbeddedHelper
    {
        public static DiscordEmbed? CreateDefaultEmbed(string description)
        {
            return new DiscordEmbedBuilder
                {
                    Description = description,
                    Color = new Optional<DiscordColor>(DiscordColor.Purple)
                }
                .Build();
        }
    }
}