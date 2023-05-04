using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using sdb_app.services;
using Microsoft.Extensions.Logging;

namespace sdb_app.slashCommands;
public class WeatherCommandModule : ApplicationCommandModule {
    private const string NEWLINE = "\n";    
    
    [SlashCommand("weather", "Give me your location and I will give you your Weather!")]
    public async Task WeatherCommand(InteractionContext ctx, [Option("city","which city to retrieve weather for.")] string _city, [Option("country","Which country does this city belong to.")] string _country) {
        sdb_app.Program.discordClient.Logger.Log(LogLevel.Information, $"'/weather {_city} {_country}' command has been issued...", ctx);
        string content = $"Grabbing the Weather for {_city} in {_country}"+NEWLINE;
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(content));
        content += "Waiting for response from external service ..."+NEWLINE;
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
        // Create a new WeatherService instance
        WeatherService service = new WeatherService();
        // set its endpoint to that of weatherbit.io
        service.MyEndpoint = service.WeatherEndpoints.Find(x => x.Name.Equals("WeatherBit"));
        // Now output the full query
        content += "Querying " + service.MyEndpoint.Endpoint + service.MyEndpoint.CityQueryString + _city + "&" + service.MyEndpoint.CountryQueryString + _country + "&" + service.MyEndpoint.AppID + "*REDACTED*" + NEWLINE;
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
        // Get the weather for the location.
        //WeatherData weather = await service.GetWeather(_city,_country);
        string weather = await service.GetWeather(_city,_country);
        // Display the weather information.
        // content += "The current temperature in " + _city + " is " + weather.Temperature + NEWLINE;
        // content += "The current humidity in " + _city + " is " + weather.Humidity + NEWLINE;
        // content += "The current wind speed in " + _city + " is " + weather.WindSpeed + NEWLINE;
        content += "This is the raw response : " + NEWLINE + "`"+weather+"`"+NEWLINE;
        content += "~~~" + NEWLINE;
        content += "Thanks for waiting!";
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
    }
}