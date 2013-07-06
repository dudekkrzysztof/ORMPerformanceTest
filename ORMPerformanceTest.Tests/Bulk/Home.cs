using System;

namespace ORMPerformanceTest.Tests.Bulk
{
    public class Home
    {
        public int Id { get; set; }
        public int Surface { get; set; }
        public double Price { get; set; }
        public int BuildYear { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public int ProvinceId { get; set; }
        public DateTime AddTime { get; set; } 
    }
}