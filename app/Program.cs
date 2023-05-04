using System.IO;
using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using System.Reflection;
using sdb_app.slashCommands;

namespace sdb_app;
class Program
{
    
    /// <summary>
    /// This denotes the file with a plaintext discord token for your bot
    /// </summary>
    //static string TokenFile = "discord_token.secret";
    static string DiscordTokenFile = "discord_token.secret";
    static string WeatherTokenFile = "weather_token.secret";
    /// <summary>
    /// Token Validator
    /// </summary>
    /// <returns>Either a token-string or string.Empty when failed.</returns>
    static string GetToken(string _tokenFile) {
        if (File.Exists(_tokenFile)) {
            var token = File.ReadAllText(_tokenFile);
            return token;
        } else return string.Empty;
    }
    private static string _weatherToken = String.Empty;
    internal static string WeatherToken { get; } = GetToken(WeatherTokenFile);
    internal static DiscordClient? discordClient { get; set; }

    static async Task Main(string[] args)
    {
        var tokenResult = GetToken(DiscordTokenFile);
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
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Information
            };
            // Creating a new discord client with config
            discordClient = new DiscordClient(discordConfig);
            
            // creating a CommandsNext Configuration and pass it into our client
            var cmdsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            };
            var commands = discordClient.UseCommandsNext(cmdsConfig);
            var slashCommands = discordClient.UseSlashCommands();
            slashCommands.RegisterCommands<BothelpCommandModule>();
            slashCommands.RegisterCommands<PromoteCommandModule>();
            slashCommands.RegisterCommands<WeatherCommandModule>();
            
            commands.RegisterCommands(Assembly.GetExecutingAssembly());
            // try to connect
            await discordClient.ConnectAsync();
            // and wait infinitly
            await Task.Delay(-1);
        }
    }
}
