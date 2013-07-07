using System;
using System.Collections.Generic;
using System.Linq;
using ORMPerformanceTest.TestData;
using ORMPerformanceTest.Tests.Bulk;
using ORMPerformanceTest.Tests.Helpers;

namespace ORMPerformanceTest.Tests.EF
{
    public class TestEF : ITest
    {
        public TestEF()
        {
            TestKind = "Entity CF";
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
            using (var ctx = new Context(connectionString))
            {
                if (ctx.Database.Exists())
                    ctx.Database.Delete();
                ctx.Database.Initialize(true);
                List<Province> entityProwinces = new List<Province>();
                foreach (var province in ProvinceData.GetProvinces())
                {
                    var prow = new Province { Code = province.Code, Name = province.Name };
                    ctx.Provinces.Add(prow);
                    ctx.SaveChanges();
                    entityProwinces.Add(prow);
                }

                BulkUploadToSql bulk =
                    BulkUploadToSql.Load(
                        HomeData.GetHomes()
                            .Select(
                                i =>
                                    new Bulk.Home
                                    {
                                        AddTime = DateTime.Now,
                                        BuildYear = i.BuildYear,
                                        City = i.City,
                                        Description = i.Description,
                                        Price = i.Price,
                                        Surface = i.Surface,
                                        ProvinceId = entityProwinces.Single(j => j.Code == i.HomeProvince.Code).Id
                                    }), "Home", 10000, connectionString);
                bulk.Flush();

            }

        }

        #region [private]
        private static int CountTest(string connectionString)
        {
            int count;
            using (var ctx = new Context(connectionString))
            {
                count = ctx.Homes.Count();
            }
            return 1;
        }
        private static int SelectAllTest(string connectionString)
        {
            List<Home> count;
            try
            {
                using (var ctx = new Context(connectionString))
                {
                    count = ctx.Homes.ToList();
                }
            }
            catch (OutOfMemoryException)
            {
                return 0;
            }
            return count.Count;
        }
        private static int SelectPartTest(string connectionString)
        {
            List<Home> count;
            try
            {
                using (var ctx = new Context(connectionString))
                {
                    count = ctx.Homes.Where(h => h.BuildYear < 2000).ToList();
                }
            }
            catch (OutOfMemoryException)
            {
                return 0;
            }
            return count.Count;
        }
        private static int SelectJoinTest(string connectionString)
        {
            List<Home> count;
            try
            {
                using (var ctx = new Context(connectionString))
                {
                    count = ctx.Homes.Where(h => h.HomeProvince.Code == 10).ToList();
                }
            }
            catch (OutOfMemoryException)
            {
                return 0;
            }
            return count.Count;
        }
        private static int Update100Test(string connectionString)
        {
            int count;
            using (var ctx = new Context(connectionString))
            {
                var homes = ctx.Homes.Where(h => h.BuildYear == 2014);
                count = homes.Count();
                foreach (var home in homes)
                {
                    if (home != null)
                    {
                        home.BuildYear = 2015;
                        ctx.SaveChanges();
                    }
                }
            }
            return count;
        }
        private static int Delete100Test(string connectionString)
        {
            int count;
            using (var ctx = new Context(connectionString))
            {
                var homes = ctx.Homes.Where(h => h.BuildYear == 2015);
                count = homes.Count();
                foreach (var home in homes)
                {
                    if (home != null)
                    {
                        ctx.Homes.Remove(home);
                    }
                }
                ctx.SaveChanges();
            }
            return count;
        }
        private static int InsertTest(string connectionStrning)
        {
            using (var ctx = new Context(connectionStrning))
            {
                var entityProwinces = ctx.Provinces.ToList();

                foreach (var home in HomeData.Get100Homes())
                {
                    ctx.Homes.Add(new Home
                    {
                        BuildYear = home.BuildYear,
                        City = home.City,
                        Description = home.Description,
                        HomeProvince = entityProwinces.Find(i => i.Code == home.HomeProvince.Code),
                        Price = (decimal)home.Price,
                        Surface = home.Surface,
                        AddTime = DateTime.Now
                    });
                    ctx.SaveChanges();
                }
            }
            return 100;
        }
        #endregion [private]
    }
}