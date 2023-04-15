using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;


public class StopCommandModule : BaseCommandModule {
    [Command("stop")]
    public async Task StopCommand(CommandContext ctx) {
        await ctx.RespondAsync("Now shutting down the bot!.");
        System.Environment.Exit(0);
    }
}