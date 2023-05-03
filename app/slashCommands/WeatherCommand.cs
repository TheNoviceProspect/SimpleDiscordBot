using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using sdb_app.services;

namespace sdb_app.slashCommands;
public class WeatherCommandModule : ApplicationCommandModule {
    private const string NEWLINE = "\n";    
    
    [SlashCommand("weather", "Give me your location and I will give you your Weather!")]
    public async Task WeatherCommand(InteractionContext ctx, [Option("location","which location to retrieve weather for.")] string _location) {
        string content = "";
        content = $"Grabbing the Weather for {_location}";
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(content));
        content += NEWLINE;
        content += "Waiting for response from external service ...";
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
        await Task.Delay(1500);
        // Do the work here
        
        // Get the weather for the location.
        WeatherData weather = await WeatherService.GetWeather(_location);

        // Display the weather information.
        content += "The current temperature in " + _location + " is " + weather.Temperature + NEWLINE;
        content += "The current humidity in " + _location + " is " + weather.Humidity + NEWLINE;
        content += "The current wind speed in " + _location + " is " + weather.WindSpeed + NEWLINE;
        content += NEWLINE;
        content += "Thanks for waiting!";
        content += "The weather is what it is";
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
    }
}