using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ORMPerformanceTest.Tests.Helpers;
using PetaPoco;
using Simple.Data;
using Simple.Data.Ado;
using Simple.Data.SqlServer;
using Database = Simple.Data.Database;

namespace ORMPerformanceTest.Tests.Simple.Data
{
    public class TestSimpleData : ITest
    {
        public TestSimpleData()
        {
            TestKind = "Simple.Data";
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
            var ado = new AdoAdapter();
            var sql = new SqlQueryPager();
            var db = Database.OpenConnection(connectionString);
            var prowinceDictionary = new Dictionary<int, int>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(Const.DBCreateScript, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            foreach (var province in ORMPerformanceTest.TestData.ProvinceData.GetProvinces())
            {
                db.Province.Insert(new { Name = province.Name, Code = province.Code });
            }
            var provinces = db.Province.All().ToList();
            foreach (var province in provinces)
            {
                prowinceDictionary.Add(province.Code, province.Id);
            }
            Bulk.BulkUploadToSql bulk =
                   Bulk.BulkUploadToSql.Load(
                       TestData.HomeData.GetHomes()
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
                                       ProvinceId = prowinceDictionary[i.HomeProvince.Code],
                                   }), "Home", 10000, connectionString);
            bulk.Flush();
        }

        #region [Private]
        private static int CountTest(string connectionString)
        {
            var db = Database.OpenConnection(connectionString);
            long count = db.Home.GetCount();
            return 1;
        }

        private static int SelectAllTest(string connectionString)
        {
            var db = Database.OpenConnection(connectionString);
            int count = db.Home.All().ToList().Count;
            return count;
        }

        private static int SelectPartTest(string connectionString)
        {
            var db = Database.OpenConnection(connectionString);
            int count = db.Home.FindAll(db.Home.BuildYear < 2000).ToList().Count;
            return count;
        }

        private static int SelectJoinTest(string connectionString)
        {
            var db = Database.OpenConnection(connectionString);
            int count = db.Province.Home.FindAll(db.Province.Code == 10).ToList().Count;
            return count;
        }

        private static int InsertTest(string connectionString)
        {
            var prowinceDictionary = new Dictionary<int, int>();
            var db = Database.OpenConnection(connectionString);
            var provinces = db.Province.All().ToList();
            foreach (var province in provinces)
            {
                prowinceDictionary.Add(province.Code, province.Id);
            }

            foreach (var home in ORMPerformanceTest.TestData.HomeData.Get100Homes())
            {
                db.Home.Insert(new
                {
                    BuildYear = home.BuildYear,
                    City = home.City,
                    Description = home.Description,
                    ProvinceId = prowinceDictionary[home.HomeProvince.Code],
                    Price = home.Price,
                    Surface = home.Surface,
                    AddTime = DateTime.Now
                });
            }
            return 100;
        }

        private static int Update100Test(string connectionString)
        {
            var db = Database.OpenConnection(connectionString);
            db.Home.UpdateAll(BuildYear: 2015, Condition: db.Home.BuildYear == 2014);
            return 100;
        }

        private static int Delete100Test(string connectionString)
        {
            var db = Database.OpenConnection(connectionString);
            db.Home.DeleteByBuildYear(2015);
            return 100;
        }
        #endregion [Private]

    }
}