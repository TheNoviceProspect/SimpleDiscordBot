using System.IO;
using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Reflection;

namespace sdb_app;
class Program
{
    /// <summary>
    /// This denotes the file with a plaintext discord token for your bot
    /// </summary>
    //static string TokenFile = "discord_token.secret";
    static string TokenFile = "/home/smzb/workspace/discord_token.secret";
    /// <summary>
    /// Token Validator
    /// </summary>
    /// <returns>Either a token-string or string.Empty when failed.</returns>
    static string GetToken() {
        if (File.Exists(TokenFile)) {
            var token = File.ReadAllText(TokenFile);
            return token;
        } else return string.Empty;
    }

    static async Task Main(string[] args)
    {
        var tokenResult = GetToken();
        // check wether the token was empty and exit the app with a failure if so
        if (tokenResult == String.Empty) {
            Console.WriteLine("There is no token present!");
            System.Environment.Exit(1);
        } else {
            // Creating a new discord config to pass into the discord client
            var discordConfig = new DiscordConfiguration()
            {
                Token = tokenResult,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            };
            // Creating a new discord client with config
            var discordClient = new DiscordClient(discordConfig);
            // creating a CommandsNext Configuration and pass it into our client
            var cmdsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            };
            var commands = discordClient.UseCommandsNext(cmdsConfig);
            
            commands.RegisterCommands(Assembly.GetExecutingAssembly());
            // try to connect
            await discordClient.ConnectAsync();
            // and wait infinitly
            await Task.Delay(-1);
        }
    }
}
