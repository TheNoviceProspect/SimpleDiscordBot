using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using sdb_app.services;

namespace sdb_app.slashCommands;
public class WeatherCommandModule : ApplicationCommandModule {
    private const string NEWLINE = "\n";    
    
    [SlashCommand("weather", "Give me your location and I will give you your Weather!")]
    public async Task WeatherCommand(InteractionContext ctx, [Option("city","which city to retrieve weather for.")] string _city, [Option("country","Which country does this city belong to.")] string _country) {
        string content = "";
        content = $"Grabbing the Weather for {_city} in {_country}";
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(content));
        content += NEWLINE;
        content += "Waiting for response from external service ...";
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
        // Get the weather for the location.
        WeatherService service = new WeatherService();
        service.MyEndpoint = service.WeatherEndpoints.Find(x => x.Name.Equals("WeatherBit"));
        content += "Querying " + service.MyEndpoint.Endpoint;
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
        WeatherData weather = await service.GetWeather(_city,_country);
        // Display the weather information.
        content += "The current temperature in " + _city + " is " + weather.Temperature + NEWLINE;
        content += "The current humidity in " + _city + " is " + weather.Humidity + NEWLINE;
        content += "The current wind speed in " + _city + " is " + weather.WindSpeed + NEWLINE;
        content += NEWLINE;
        content += "Thanks for waiting!";
        content += "The weather is what it is";
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
    }
}