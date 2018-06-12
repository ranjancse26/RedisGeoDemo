namespace RedisGeoDemo
{
    public class ResturantGeo
    {
        public ResturantGeo() { }

        public ResturantGeo(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
