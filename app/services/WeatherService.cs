using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace sdb_app.services;
public class WeatherData
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string WindSpeed { get; set; }
    }

public class WeatherService {

    // I do not like this, but fine...
    const string WeatherUnderground = "https://api.wunderground.com/api/";
    const string OpenWeatherMap = "https://api.openweathermap.org/data/2.5/weather?";


/// <summary>
/// A "simple" http query to retrieve weather data from an api endpoint
/// </summary>
/// <param name="location">Any city name should do, if none is given London, UK is assumed</param>
/// <returns>an asynchronous result containing <see>WeatherData</see></returns>
public static async Task<WeatherData> GetWeather(string location = "London")
{
    // Get the API key from Weather Underground.
    string apiKey = "YOUR_API_KEY";

    // Create a new HttpClient object.
    HttpClient client = new HttpClient();

    // Set the Content-Type header to application/x-www-form-urlencoded.
    client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

    // Build the query string.
    string queryString = "q=" + location + "&appid=" + apiKey;

    // Make the request to the Weather Underground API.
    HttpResponseMessage response = await client.GetAsync(OpenWeatherMap + queryString);

    // Check the response status code.
    if (response.StatusCode == HttpStatusCode.OK)
    {
        // Get the weather data from the response body.
        string rawWeatherData = await response.Content.ReadAsStringAsync();

        // Parse the weather data into a Weather object.
        WeatherData weather = JsonConvert.DeserializeObject<WeatherData>(rawWeatherData);

        return weather;
    }
    else
    {
        // Handle the error.
        throw new Exception("Error: " + response.StatusCode);
    }
}

}