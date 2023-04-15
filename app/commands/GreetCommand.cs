using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace sdb_app.commands;

public class GreetCommandModule : BaseCommandModule {
    [Command("greet")]
    public async Task GreetCommand(CommandContext ctx, [RemainingText] string name) {
        await ctx.RespondAsync($"Hello {name}, good to see you here..");
    }
    [Command("greet")]
    public async Task GreetCommand(CommandContext ctx, DiscordMember member) {
        await ctx.RespondAsync($"Hello {member.Mention}, good to see you here..");
    }
}