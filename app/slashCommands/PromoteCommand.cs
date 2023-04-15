using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
namespace sdb_app.slashCommands;

public class PromoteCommandModule : ApplicationCommandModule {
    private const string NEWLINE = "\n";    
    
    [SlashCommand("promote", "makes the member a group of the indicated role.")]
    public async Task Promote(InteractionContext ctx, [Option("user", "User to promote")] DiscordUser user)
    {
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Promoted {user.Username} "));
    }

    [SlashCommand("list-roles", "print a list of roles")]
    public async Task ListRoles(InteractionContext ctx)
    {
        string content = "";
        foreach (var role in ctx.Guild.Roles.Values)
        {
            content += role;
            content += "|";
        }
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"All Roles : {content}"));
    }
}