using System.Collections.Generic;

namespace ORMPerformanceTest.TestData
{
    public static class ProvinceData
    {
        public static IEnumerable<Province> GetProvinces()
        {
            yield return new Province { Name = "dolnośląskie", Code = 2 };
            yield return new Province { Name = "kujawsko-pomorskie", Code = 4 };
            yield return new Province { Name = "lubelskie", Code = 06 };
            yield return new Province { Name = "lubuskie", Code = 08 };
            yield return new Province { Name = "łódzkie", Code = 10 };
            yield return new Province { Name = "małopolskie", Code = 12 };
            yield return new Province { Name = "mazowieckie", Code = 14 };
            yield return new Province { Name = "opolskie", Code = 16 };
            yield return new Province { Name = "podkarpackie", Code = 18 };
            yield return new Province { Name = "podlaskie", Code = 20 };
            yield return new Province { Name = "pomorskie", Code = 22 };
            yield return new Province { Name = "śląskie", Code = 24 };
            yield return new Province { Name = "świętokrzyskie", Code = 26 };
            yield return new Province { Name = "warmińsko-mazurskie", Code = 28 };
            yield return new Province { Name = "wielkopolskie", Code = 30 };
            yield return new Province { Name = "zachodniopomorskie", Code = 32 };
        }
    }
}