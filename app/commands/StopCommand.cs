using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace sdb_app.commands;

public class StopCommandModule : BaseCommandModule {
    [Command("stop")]
    public async Task StopCommand(CommandContext ctx) {
        sdb_app.Program.discordClient.Logger.Log(LogLevel.Information, "'!stop' command has been issued...", ctx);
        await ctx.RespondAsync("Now shutting down the bot!.");
        System.Environment.Exit(0);
    }
}