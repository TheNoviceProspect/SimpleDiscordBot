using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using sdb_app.services;
using Microsoft.Extensions.Logging;
using sdb_app.services.data;

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
        // Get the weather for the location through our specific endpoint
        WeatherData weather = await service.GetWeather(service.MyEndpoint, _city,_country);
        // Simplify data access
        var data = weather.data[0];
        // Display the weather information.
        content += "The current temperature in " + data.city_name + "," + data.country_code + " is " + data.temp + "°C" + NEWLINE;
        content += "The current humidity is " + data.rh + "% and the current wind speed is " + data.wind_spd + "m/s hailing from " + data.wind_cdir_full + NEWLINE;
        content += "~~~" + NEWLINE;
        content += "Thanks for waiting!";
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(content));
    }
}