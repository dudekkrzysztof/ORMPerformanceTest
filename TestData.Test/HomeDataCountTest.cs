using System.Linq;
using NUnit.Framework;
using ORMPerformanceTest.TestData;

namespace TestData.Test
{
    [TestFixture]
    public class HomeDataCountTest
    {


        [Test]
        public void HomeData_Count_ShouldBe3141504()
        {
            int count;

            count = HomeData.GetHomes().Count();

            Assert.That(count, Is.EqualTo(HomeData.HomeNumber));
        }
    }
}
