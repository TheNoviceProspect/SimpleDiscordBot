using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace sdb_app.commands;

public class PingCommandModule : BaseCommandModule {
    [Command("ping")]
    public async Task PingCommand(CommandContext ctx) {
        sdb_app.Program.discordClient.Logger.Log(LogLevel.Information, "'!ping' command has been issued...", ctx);
        await ctx.RespondAsync("This is not a ping pong match!");
    }
}