using Geolocation;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedisGeoDemo
{
    class Program
    {
        private static double Latitude { get; set; }
        private static double Longitude { get; set; }

        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RedisConnection"].ConnectionString;

            Console.WriteLine("Loading Resturants..");
            var redisList = ImportResturantData(connectionString);

            Console.WriteLine("Searching Resturants within 25 miles");

            Latitude = 36.0586094;
            Longitude = -78.9279599;

            var filteredResturants = SearchResturantsWithInSpecifiedMiles(redisList, 25);

            Console.WriteLine("Completed Searching for Resturants..");
            Console.ReadLine();
        }

        private static List<Restaurant> SearchResturantsWithInSpecifiedMiles(RedisList<Restaurant> restaurants, int miles)
        {
            var boundaries = new CoordinateBoundaries(Latitude, Longitude, miles);
            var filteredRestaurants = new List<Restaurant>();

            Parallel.ForEach(restaurants, (restaurant) => {
                if((restaurant.ResturantGeo.Latitude >= boundaries.Latitude 
                    && restaurant.ResturantGeo.Latitude <= boundaries.Latitude)
                    && (restaurant.ResturantGeo.Longitude >= boundaries.Longitude
                    && restaurant.ResturantGeo.Longitude <= boundaries.Longitude))
                {
                    filteredRestaurants.Add(restaurant);
                }
            });

            return filteredRestaurants;
        }

        public static RedisList<Restaurant> ImportResturantData(string connectionString)
        {
            var store = new RedisStore(connectionString);
            var redis = store.GetDatabase();

            var resturantsData = File.ReadAllText("restaurants-data.json");
            var resturantsCollection = JsonConvert.DeserializeObject<Restaurant[]>(resturantsData);

            var redisCollection = new RedisList<Restaurant>("resturants", redis);

            double latitude = 0;
            double longitude = 0;

            foreach (var resutarant in resturantsCollection)
            {
                if (!string.IsNullOrEmpty(resutarant.geolocation))
                {
                    var latLong = resutarant.geolocation.Split(';');
                    latitude = double.Parse(latLong[0]);
                    longitude = double.Parse(latLong[1]);
                }

                resutarant.ResturantGeo.Latitude = latitude;
                resutarant.ResturantGeo.Longitude = longitude;

                redisCollection.Add(resutarant);
            }

            return redisCollection;
        }
    }
}
