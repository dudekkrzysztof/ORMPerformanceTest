using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ORMPerformanceTest.Tests.EF
{
    [Table("Home")]
    public class Home
    {
        public Home()
        {
        }

        public int Id { get; set; }
        public int Surface { get; set; }
        public decimal Price { get; set; }
        public int BuildYear { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public int ProvinceId { get; set; }
        public DateTime AddTime { get; set; }

        [ForeignKey("ProvinceId")]
        public virtual Province HomeProvince { get; set; }
    }
}