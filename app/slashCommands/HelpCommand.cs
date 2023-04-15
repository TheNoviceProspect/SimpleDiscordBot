using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
namespace sdb_app.slashCommands;
public class BothelpCommandModule : ApplicationCommandModule {
    [SlashCommand("bothelp", "This should return a help text of sorts.")]
    public async Task BothelpCommand(InteractionContext ctx) {
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Success!"));
    }
}