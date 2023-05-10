using System.IO;
using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using System.Reflection;
using sdb_app.slashCommands;
using Microsoft.Extensions.Logging;

namespace sdb_app;
class Program
{
        /// <summary>
    /// Token File Parser
    /// </summary>
    /// <returns>Either a token-string or string.Empty when failed.</returns>
    static string GetToken(string _tokenFile) {
        string properName = _tokenFile + "_token.secret";
        if (File.Exists(properName)) {
            var token = File.ReadAllText(properName);
            return token.TrimEnd('\n');
        } else return string.Empty;
    }
    private static string _weatherToken = String.Empty;
    internal static string WeatherToken { get; } = GetToken("weather");
    internal static DiscordClient? discordClient { get; set; }

    private static AssemblyConfigurationAttribute? assemblyConfigurationAttribute = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
    private static string? buildConfigurationName = assemblyConfigurationAttribute?.Configuration;

    internal static bool IsDebug { get; } = (buildConfigurationName == "Debug") ? true : false;


    static async Task Main(string[] args)
    {
        var tokenResult = GetToken("discord");
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
            if (IsDebug)
            {
                discordClient.Logger.Log(LogLevel.Information, $"This app was build using the '{buildConfigurationName}' configuration..", assemblyConfigurationAttribute);
            }
            // try to connect
            await discordClient.ConnectAsync();
            // and wait infinitly
            await Task.Delay(-1);
        }
    }
}
