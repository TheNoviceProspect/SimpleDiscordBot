using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace sdb_app.commands;

public class StopCommandModule : BaseCommandModule {
    [Command("stop")]
    public async Task StopCommand(CommandContext ctx) {
        await ctx.RespondAsync("Now shutting down the bot!.");
        System.Environment.Exit(0);
    }
}