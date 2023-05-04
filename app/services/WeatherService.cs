using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace sdb_app.services;

/// <summary>
/// This is a container for Weather Data
/// </summary>
public class WeatherData
    {
        public string? Temperature { get; set; }
        public string? Humidity { get; set; }
        public string? WindSpeed { get; set; }
    }

/// <summary>
/// The Weather Service class
/// </summary>
public class WeatherService {

    /// <summary>
    /// Describe how a weather endpoint functions
    /// </summary>
    public struct WeatherEndpoint
    {
        /// <summary>
        /// The endpoint name
        /// </summary>
        public string Name;
        /// <summary>
        /// The full url to the endpoint without any query parameters
        /// </summary>
        public string Endpoint;
        /// <summary>
        /// How to query a location/city
        /// </summary>
        public string CityQueryString;
        /// <summary>
        /// How to query a country
        /// </summary>
        public string CountryQueryString;
        /// <summary>
        /// How to pass my app secret?
        /// </summary>
        public string AppID;

        /// <summary>
        /// WeatherEndpoint constructor simply assign its parameters to an object of type WeatherEndpoint
        /// </summary>
        /// <param name="_name">The endpoint name</param>
        /// <param name="_endpoint">The full url to the endpoint without any query parameters</param>
        /// <param name="_cityquerystring">How to query a location/city</param>
        /// <param name="_countryquerystring">How to query a country</param>
        /// <param name="_appId">How to pass my app secret?</param>
        public WeatherEndpoint(string _name, string _endpoint, string _cityquerystring, string _countryquerystring, string _appId)
        {
            Name = _name;
            Endpoint = _endpoint;
            CityQueryString = _cityquerystring;
            CountryQueryString = _countryquerystring;
            AppID = _appId;
        }
    }

    /// <summary>
    /// A list of WeatherEndpoints
    /// </summary>
    /// <typeparam name="WeatherEndpoint">A struct on how to talk to a weather endpoint</typeparam>
    /// <returns>an empty list of endpoints before population</returns>
    public List<WeatherEndpoint> WeatherEndpoints = new List<WeatherEndpoint>();

    /// <summary>
    /// Let's construct our WeatherEndpoints with known values for 3 different providers.
    /// </summary>
    public WeatherService()
    {
        WeatherEndpoints.Add(new WeatherEndpoint("WeatherUnderground", "https://api.wunderground.c4om/api/", "q=", "", "appid="));
        WeatherEndpoints.Add(new WeatherEndpoint("OpenWeatherMap", "https://api.openweathermap.org/data/2.5/weather?", "q=", "", "appid="));
        WeatherEndpoints.Add(new WeatherEndpoint("WeatherBit", "https://api.weatherbit.io/v2.0/current?", "city=", "country=", "key="));
    }

    public WeatherEndpoint MyEndpoint { get; set; }

    /// <summary>
    /// A "simple" http query to retrieve weather data from an api endpoint
    /// </summary>
    /// <param name="_city">Any city name should do, if none is given London, UK is assumed</param>
    /// <returns>an asynchronous result containing <see>WeatherData</see></returns>
    public async Task<String> GetWeather(string _city = "London", string _country = "UK") {
        // Get the API key from Weather Underground.
        string apiKey = sdb_app.Program.WeatherToken;

        // Build the query string depending on wether the endpoint supports countries or not.
        string _queryString = String.Empty;
        string _debugQueryString = String.Empty;
        if (MyEndpoint.CountryQueryString=="")
        {
            _queryString = $"{MyEndpoint.Endpoint}{MyEndpoint.CityQueryString}{_city}&{MyEndpoint.AppID}{apiKey}";
            // make sure there is key when we say "redacted" or "empty" when it is
            if (apiKey==String.Empty)
            {
                _debugQueryString = $"{MyEndpoint.Endpoint}{MyEndpoint.CityQueryString}{_city}&{MyEndpoint.AppID}[EMPTY]";
            } else
            {
                _debugQueryString = $"{MyEndpoint.Endpoint}{MyEndpoint.CityQueryString}{_city}&{MyEndpoint.AppID}[REDACTED]";
            }
        
        } else
        {
            _queryString = $"{MyEndpoint.Endpoint}{MyEndpoint.CityQueryString}{_city}&{MyEndpoint.CountryQueryString}{_country}&{MyEndpoint.AppID}{apiKey}";
            // make sure there is key when we say "redacted" or "empty" when it is
            if (apiKey == String.Empty)
            {
                _debugQueryString = $"{MyEndpoint.Endpoint}{MyEndpoint.CityQueryString}{_city}&{MyEndpoint.CountryQueryString}{_country}&{MyEndpoint.AppID}[EMPTY]";
            }
            else
            {
                _debugQueryString = $"{MyEndpoint.Endpoint}{MyEndpoint.CityQueryString}{_city}&{MyEndpoint.CountryQueryString}{_country}&{MyEndpoint.AppID}[REDACTED]";
            }
        }

        // Log the entire query
        Program.discordClient.Logger.Log(LogLevel.Information, $"This is the full query: {_debugQueryString}", MyEndpoint);

        // Create a new HttpClient object.
        HttpClient client = new HttpClient();

        // Set the Content-Type header to application/x-www-form-urlencoded.
        //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
        client.DefaultRequestHeaders.Add("Content-Type", "application/json");

        // Make the request to the Weather API.
        //HttpResponseMessage response = await client.GetAsync(MyEndpoint.Endpoint + _queryString);
        
        // Create the request before sending it to the server.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _queryString);

        Program.discordClient.Logger.Log(LogLevel.Information, $"This is the api request: {request.ToString}", MyEndpoint);

        // Send the request
        HttpResponseMessage response = await client.SendAsync(request);

        // Check the response status code.
        if (response.StatusCode == HttpStatusCode.OK)
        {
            // Get the weather data from the response body.
            string rawWeatherData = await response.Content.ReadAsStringAsync();
            sdb_app.Program.discordClient.Logger.Log(LogLevel.Information,"We have received a response as follows", rawWeatherData);

            // Parse the weather data into a Weather object.
            // TODO(smzb): this needs addressed since the json response is more "layered"
            //WeatherData? weather = JsonConvert.DeserializeObject<WeatherData>(rawWeatherData);

            return rawWeatherData;
        }
        else
        {
            // Handle the error.
            throw new Exception("Error: " + response.StatusCode);
        }
    }

}
