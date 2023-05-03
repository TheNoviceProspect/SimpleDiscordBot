using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace sdb_app.services;
public class WeatherData
    {
        public string? Temperature { get; set; }
        public string? Humidity { get; set; }
        public string? WindSpeed { get; set; }
    }

public class WeatherService {

    public struct WeatherEndpoint
    {
        public string Name;
        public string Endpoint;
        public string CityQueryString;
        public string CountryQueryString;
        public string AppID;

        public WeatherEndpoint(string _name, string _endpoint, string _cityquerystring, string _countryquerystring, string _appId)
        {
            Name = _name;
            Endpoint = _endpoint;
            CityQueryString = _cityquerystring;
            CountryQueryString = _countryquerystring;
            AppID = _appId;
        }
    }

    public List<WeatherEndpoint> WeatherEndpoints = new List<WeatherEndpoint>();

    public WeatherService()
    {
        WeatherEndpoints.Add(new WeatherEndpoint("WeatherUnderground", "https://api.wunderground.com/api/", "q=", "", "appid="));
        WeatherEndpoints.Add(new WeatherEndpoint("OpenWeatherMap", "https://api.openweathermap.org/data/2.5/weather?", "q=", "", "appid="));
        WeatherEndpoints.Add(new WeatherEndpoint("WeatherBit", "https://api.weatherbit.io/v2.0/current?", "city=", "country=", "key="));
    }

    public WeatherEndpoint MyEndpoint { get; set; }

    /// <summary>
    /// A "simple" http query to retrieve weather data from an api endpoint
    /// </summary>
    /// <param name="_city">Any city name should do, if none is given London, UK is assumed</param>
    /// <returns>an asynchronous result containing <see>WeatherData</see></returns>
    public async Task<WeatherData> GetWeather(string _city = "London", string _country = "UK") {
        // Get the API key from Weather Underground.
        string apiKey = sdb_app.Program.WeatherToken;

        string _queryString = String.Empty;

        // Build the query string depending on wether the endpoint supports countries or not.
        if (MyEndpoint.CountryQueryString=="")
        {
            _queryString = $"{MyEndpoint.Endpoint}{MyEndpoint.CityQueryString}{_city}&{MyEndpoint.AppID}{apiKey}";
        } else
        {
            _queryString = $"{MyEndpoint.Endpoint}{MyEndpoint.CityQueryString}{_city}&{MyEndpoint.CountryQueryString}{_country}&{MyEndpoint.AppID}{apiKey}";
        }

        Program.discordClient.Logger.Log(LogLevel.Debug, $"This is the full query: {_queryString}", new object());

        // Create a new HttpClient object.
        HttpClient client = new HttpClient();

        // Set the Content-Type header to application/x-www-form-urlencoded.
        //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
        client.DefaultRequestHeaders.Add("Content-Type", "application/json");

        // Make the request to the Weather Underground API.
        //HttpResponseMessage response = await client.GetAsync(MyEndpoint.Endpoint + _queryString);
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _queryString);

        // Send the request
        HttpResponseMessage response = await client.SendAsync(request);

        // Check the response status code.
        if (response.StatusCode == HttpStatusCode.OK)
        {
            // Get the weather data from the response body.
            string rawWeatherData = await response.Content.ReadAsStringAsync();

            // Parse the weather data into a Weather object.
            WeatherData? weather = JsonConvert.DeserializeObject<WeatherData>(rawWeatherData);

            return weather;
        }
        else
        {
            // Handle the error.
            throw new Exception("Error: " + response.StatusCode);
        }
    }

}