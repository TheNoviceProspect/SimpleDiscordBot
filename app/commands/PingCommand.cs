using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

public class PingCommandModule : BaseCommandModule {
    [Command("ping")]
    public async Task PingCommand(CommandContext ctx) {
        await ctx.RespondAsync("This is not a ping pong match!");
    }
}