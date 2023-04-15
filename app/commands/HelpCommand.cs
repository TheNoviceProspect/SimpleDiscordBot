using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

public class BothelpCommandModule : BaseCommandModule {
    [Command("bothelp")]
    public async Task BothelpCommand(CommandContext ctx) {
        await ctx.RespondAsync("This should return a help text of sorts.");
    }
}