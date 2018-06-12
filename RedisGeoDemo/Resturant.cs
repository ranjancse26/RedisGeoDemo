namespace RedisGeoDemo
{
    public class Restaurant
    {
        public Restaurant()
        {
            ResturantGeo = new ResturantGeo();
        }

        public int id { get; set; }
        public string premise_name { get; set; }
        public string premise_address1 { get; set; }
        public string premise_address2 { get; set; }
        public string premise_city { get; set; }
        public string premise_state { get; set; }
        public string premise_zip { get; set; }
        public string premise_phone { get; set; }
        public string hours_of_operation { get; set; }
        public string opening_date { get; set; }
        public string closing_date { get; set; }
        public int? seats { get; set; }
        public string water { get; set; }
        public string sewage { get; set; }
        public int? insp_freq { get; set; }
        public string est_group_desc { get; set; }
        public int? risk { get; set; }
        public string smoking_allowed { get; set; }
        public string type_description { get; set; }
        public string rpt_area_desc { get; set; }
        public string status { get; set; }
        public string transitional_type_desc { get; set; }
        public string geolocation { get; set; }
        public ResturantGeo ResturantGeo { get; set; }
    }
}
