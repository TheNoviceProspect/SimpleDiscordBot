/// <summary>
/// This is essentially a class representation of a Weatherbit.io json response
/// Most of this is from https://www.weatherbit.io/api/weather-current
/// </summary>

namespace sdb_app.services.data {
    public class WeatherData
    {
        public int? count { get; set; }
        /// <summary>
        /// In general one would only expect one item in this array
        /// </summary>
        /// <value>Always refer to data[0].PROPERTY</value>
        public Datum[]? data { get; set; }
    }

    public class Datum
    {
        public float? app_temp { get; set; }
        public int? aqi { get; set; }
        public string? city_name { get; set; }
        public int? clouds { get; set; }
        public string? country_code { get; set; }
        public string? datetime { get; set; }
        public float? dewpt { get; set; }
        public float? dhi { get; set; }
        public float? dni { get; set; }
        public float? elev_angle { get; set; }
        public float? ghi { get; set; }
        public float? gust { get; set; }
        public float? h_angle { get; set; }
        public float? lat { get; set; }
        public float? lon { get; set; }
        public string? ob_time { get; set; }
        public string? pod { get; set; }
        public float? precip { get; set; }
        public float? pres { get; set; }
        public int? rh { get; set; }
        public float? slp { get; set; }
        public int? snow { get; set; }
        public float? solar_rad { get; set; }
        public string[]? sources { get; set; }
        public string? state_code { get; set; }
        public string? station { get; set; }
        public string? sunrise { get; set; }
        public string? sunset { get; set; }
        public float? temp { get; set; }
        public string? timezone { get; set; }
        public int? ts { get; set; }
        public float? uv { get; set; }
        public int? vis { get; set; }
        public Weather? weather { get; set; }
        public string? wind_cdir { get; set; }
        public string? wind_cdir_full { get; set; }
        public int? wind_dir { get; set; }
        public float? wind_spd { get; set; }
    }

    public class Weather
    {
        public string? description { get; set; }
        public int? code { get; set; }
        public string? icon { get; set; }
    }
}