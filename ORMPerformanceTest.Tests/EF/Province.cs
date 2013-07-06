using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ORMPerformanceTest.Tests.EF
{
    [Table("Province")]
    public class Province
    {
        public Province()
        {
        }

        public int Id { set; get; }
        public string Name { get; set; }
        public int Code { get; set; }
        public IEnumerable<Home> Homes { get; set; }
    }
}