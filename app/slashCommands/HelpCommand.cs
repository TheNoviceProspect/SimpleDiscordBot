using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
namespace sdb_app.slashCommands;
public class BothelpCommandModule : ApplicationCommandModule {
    private const string NEWLINE = "\n";    
    
    [SlashCommand("bothelp", "This should return a help text of sorts.")]
    public async Task BothelpCommand(InteractionContext ctx) {
        string content = "";
        content = "Starting some arduous task!";
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(content));
        content += NEWLINE;
        content += "Now waiting for 1.5 seconds!";
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
        await Task.Delay(1500);
        content += NEWLINE;
        content += "Thanks for waiting!";
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
    }
}