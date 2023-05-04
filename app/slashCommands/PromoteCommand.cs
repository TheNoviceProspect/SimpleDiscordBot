using System;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace sdb_app.slashCommands;

public class PromoteCommandModule : ApplicationCommandModule {
    private const string NEWLINE = "\n";

    internal struct ServerRole
    {
        internal UInt64 ID { get; }
        internal string Name { get; }

        internal ServerRole(UInt64 _id, string _name) {
            ID = _id;
            Name = _name;
        }
    }

    internal List<ServerRole>? ServerRoles { get; set; }

    private void EnumerateRoles(ref InteractionContext ctx) {
        ServerRoles = new List<ServerRole>{};
                
        foreach (var role in ctx.Guild.Roles.Values)
        {
            ServerRoles.Add(new ServerRole(role.Id,role.Name));
        }

    }

    private string ReturnRoles() {
        string content = String.Empty;
        foreach (var role in ServerRoles)
        {
            content += $"RoleID: {role.ID} | RoleName: {role.Name}{NEWLINE}";
        }
        return content;
    }
    
    [SlashCommand("promote", "makes the member a group of the indicated role.")]
    public async Task Promote(InteractionContext ctx, [Option("user", "User to promote")] DiscordUser user, [Option("role", "Role to promote the user to..")]  DiscordRole role)
    {
        sdb_app.Program.discordClient.Logger.Log(LogLevel.Information, $"'/promote {user} {role}' command has been issued...", ctx);
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Promoted {user.Username} to role {role.Name}"));
    }

    [SlashCommand("list-roles", "print a list of roles")]
    public async Task ListRoles(InteractionContext ctx)
    {
        sdb_app.Program.discordClient.Logger.Log(LogLevel.Information, "'/list-roles' command has been issued...", ctx);
        EnumerateRoles(ref ctx);
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"All Roles : {NEWLINE}{ReturnRoles()}"));
    }
}