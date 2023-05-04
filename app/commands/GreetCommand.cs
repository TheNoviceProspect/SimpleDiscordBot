using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace sdb_app.commands;

public class GreetCommandModule : BaseCommandModule {
    [Command("greet")]
    public async Task GreetCommand(CommandContext ctx, [RemainingText] string name) {
        sdb_app.Program.discordClient.Logger.Log(LogLevel.Information, $"'!greet {name}' command has been issued...", ctx);
        await ctx.RespondAsync($"Hello {name}, good to see you here..");
    }
    [Command("greet")]
    public async Task GreetCommand(CommandContext ctx, DiscordMember member) {
        sdb_app.Program.discordClient.Logger.Log(LogLevel.Information, $"'!greet {member.Nickname}' command has been issued...", ctx);
        await ctx.RespondAsync($"Hello {member.Mention}, good to see you here..");
    }
}