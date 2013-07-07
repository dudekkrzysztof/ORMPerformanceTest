using System;
using System.Collections.Generic;
using System.Linq;
using Massive.Model;
using ORMPerformanceTest.Tests;
using ORMPerformanceTest.Tests.Bulk;
using ORMPerformanceTest.Tests.Helpers;
using Home = Massive.Model.Home;

namespace Massive
{
    public class TestMassive : ITest
    {
        public TestMassive()
        {
            TestKind = "Massive";
        }

        public string TestKind { get; private set; }
        public TestResult Count(string connectionString)
        {
            return TestRunner.RunTest(CountTest, connectionString, string.Format("{0} {1}", TestKind, "Count"));
        }
        public TestResult Insert(string connectionString)
        {
            return TestRunner.RunTest(InsertTest, connectionString, string.Format("{0} {1}", TestKind, "Insert"));
        }
        public TestResult SelectAll(string connectionString)
        {
            return TestRunner.RunTest(SelectAllTest, connectionString, string.Format("{0} {1}", TestKind, "SelectAll"));
        }

        public TestResult SelectPart(string connectionString)
        {
            return TestRunner.RunTest(SelectPartTest, connectionString, string.Format("{0} {1}", TestKind, "SelectPart"));
        }

        public TestResult SelectJoin(string connectionString)
        {
            return TestRunner.RunTest(SelectJoinTest, connectionString, string.Format("{0} {1}", TestKind, "SelectJoin"));
        }

        public TestResult Update100(string connectionString)
        {
            return TestRunner.RunTest(Update100Test, connectionString, string.Format("{0} {1}", TestKind, "Update100"));
        }

        public TestResult Delete100(string connectionString)
        {
            return TestRunner.RunTest(Delete100Test, connectionString, string.Format("{0} {1}", TestKind, "Delete100"));
        }
        public void InitDataBase(string connectionString)
        {
            DB.Current.Execute(Const.DBCreateScript);
            Dictionary<int, int> provinces = new Dictionary<int, int>();
            var table = new Province("ORMTest");
            foreach (var province in ORMPerformanceTest.TestData.ProvinceData.GetProvinces())
            {
                var dbProvince = table.Insert(new { Code = province.Code, Name = province.Name });
                provinces.Add(province.Code, (int)dbProvince.ID);
            }
            BulkUploadToSql bulk =
                   BulkUploadToSql.Load(
                      ORMPerformanceTest.TestData.HomeData.GetHomes()
                           .Select(
                               i =>
                                   new ORMPerformanceTest.Tests.Bulk.Home
                                   {
                                       AddTime = DateTime.Now,
                                       BuildYear = i.BuildYear,
                                       City = i.City,
                                       Description = i.Description,
                                       Price = i.Price,
                                       Surface = i.Surface,
                                       ProvinceId = provinces[i.HomeProvince.Code],
                                   }), "Home", 10000, connectionString);
            bulk.Flush();
        }
        #region [Private]
        private static int CountTest(string connectionString)
        {
            var table = new Home("ORMTest");
            long count = table.Count();
            return 1;
        }

        private static int SelectAllTest(string connectionString)
        {
            var table = new Home("ORMTest");
            return table.All().Count();
        }

        private static int SelectPartTest(string connectionString)
        {
            var table = new Home("ORMTest");
            return table.All(where: "where BuildYear<@0", args: 2000).Count();
        }

        private static int SelectJoinTest(string connectionString)
        {
            var table = new Home("ORMTest");
            return table.Query(@"SELECT h.*  
from Home h
inner join Province p on h.ProvinceId=p.Id
where p.Code=@0", 10).Count();
        }

        private static int InsertTest(string connectionString)
        {
            var table = new Home("ORMTest");
            var provinces = new Province("ORMTest").All();

            foreach (var home in ORMPerformanceTest.TestData.HomeData.Get100Homes())
            {
                table.Insert(new
                {
                    BuildYear = home.BuildYear,
                    City = home.City,
                    Description = home.Description,
                    ProvinceId = provinces.First(i => i.Code == home.HomeProvince.Code).Id,
                    Price = home.Price,
                    Surface = home.Surface,
                    AddTime = DateTime.Now
                });
            }
            return 100;
        }

        private static int Update100Test(string connectionString)
        {
            var table = new Home("ORMTest");
            var toUpdate = table.All("WHERE BuildYear=@0", args: 2014).ToArray();
            int counter = 0;
            foreach (var home in toUpdate)
            {
                home.BuildYear = 2015;
                counter++;
            }
            table.Save(toUpdate);
            return counter;
        }

        private static int Delete100Test(string connectionString)
        {
            var table = new Home("ORMTest");
            return table.Delete(where: "WHERE BuildYear=@0", args: 2015);
        }
        #endregion [Private]
    }
}