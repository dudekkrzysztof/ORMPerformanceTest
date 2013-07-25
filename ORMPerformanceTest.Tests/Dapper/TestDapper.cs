using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ORMPerformanceTest.TestData;
using ORMPerformanceTest.Tests.Bulk;
using ORMPerformanceTest.Tests.Helpers;

namespace ORMPerformanceTest.Tests.Dapper
{
    [Export(typeof(ITest))]
    public class TestDapper : ITest
    {
        public TestDapper()
        {
            TestKind = "Dapper";
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
            Dictionary<int, int> provinceDictionary = new Dictionary<int, int>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(Const.DBCreateScript);
                connection.Execute("insert into Province values(@Name, @Code);"
                    , ProvinceData.GetProvinces());
                var provinces = connection.Query("select code, id from Province;");
                foreach (var province in provinces)
                {
                    provinceDictionary.Add((int)province.code, (int)province.id);
                }
                connection.Close();
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
                                        ProvinceId = provinceDictionary[i.HomeProvince.Code]
                                    }), "Home", 10000, connectionString);
            bulk.Flush();

        }

        public int Priority { get { return 2; } }

        #region [Private]
        private static int CountTest(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var homes = connection.Query("SELECT Count(*) FROM Home");
                connection.Close();
            }
            return 1;
        }

        private static int SelectAllTest(string connectionString)
        {
            var homesCount = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                homesCount = connection.Query<Home>("SELECT * FROM Home", buffered: false).Count();
                connection.Close();
            }
            return homesCount;
        }

        private static int SelectPartTest(string connectionString)
        {
            var homesCount = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                homesCount = connection.Query<Home>("SELECT * FROM Home where BuildYear<@Year"
                    , new { Year = 2000 }
                    , buffered: false).Count();
                connection.Close();
            }
            return homesCount;
        }

        private static int SelectJoinTest(string connectionString)
        {
            var homesCount = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                homesCount = connection.Query(@"SELECT h.*  
from Home h
inner join Province p on h.ProvinceId=p.Id
where p.Code=@Code", new { Code = 10 }
                    , buffered: false).Count();
                connection.Close();
            }
            return homesCount;
        }

        private static int InsertTest(string connectionString)
        {
            Dictionary<int, int> provinceDictionary = new Dictionary<int, int>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var provinces = connection.Query("select code, id from Province;");
                foreach (var province in provinces)
                {
                    provinceDictionary.Add((int)province.code, (int)province.id);
                }
                foreach (var home in HomeData.Get100Homes())
                {
                    connection.Execute("insert into Home (Surface, Price, BuildYear, City, Description, AddTime, ProvinceId) values(@Surface, @Price, @BuildYear, @City, @Description, @AddTime, @ProvinceId);"
                        , new
                        {
                            Surface = home.Surface,
                            Price = home.Price,
                            BuildYear = home.BuildYear,
                            City = home.City,
                            Description = home.Description,
                            AddTime = DateTime.Now,
                            ProvinceId = provinceDictionary[home.HomeProvince.Code]
                        });
                }
                connection.Close();
            }
            return 100;
        }

        private static int Update100Test(string connectionString)
        {
            int count;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                count = connection.Execute(@"Update Home 
set BuildYear = @NewBuildYear
where BuildYear=@OldBuildYear",
                              new
                              {
                                  NewBuildYear = 2015,
                                  OldBuildYear = 2014
                              });
                connection.Close();
            }
            return count;
        }

        private static int Delete100Test(string connectionString)
        {
            int count;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                count = connection.Execute(@"delete from Home 
where BuildYear=@BuildYear",
                        new { BuildYear = 2015 });
                connection.Close();
            }
            return count;
        }
        #endregion [Private]
    }
}