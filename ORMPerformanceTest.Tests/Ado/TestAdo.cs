using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ORMPerformanceTest.TestData;
using ORMPerformanceTest.Tests.Bulk;
using ORMPerformanceTest.Tests.Helpers;

namespace ORMPerformanceTest.Tests.Ado
{
    public class TestAdo : ITest
    {
        public TestAdo()
        {
            TestKind = "Custom ADO";
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
            return TestRunner.RunTest(UpdateTest, connectionString, string.Format("{0} {1}", TestKind, "Update100"));
        }

        public TestResult Delete100(string connectionString)
        {
            return TestRunner.RunTest(DeleteTest, connectionString, string.Format("{0} {1}", TestKind, "Delete100"));
        }

        public void InitDataBase(string connectionString)
        {
            Dictionary<int, int> provinceDictionary = new Dictionary<int, int>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open(); using (var command = new SqlCommand(Const.DBCreateScript, connection))
                {
                    command.ExecuteNonQuery();
                }
                foreach (var province in ProvinceData.GetProvinces())
                {
                    using (var command = new SqlCommand("insert into Province values(@Name, @Code);", connection))
                    {
                        command.Parameters.AddWithValue("@Name", province.Name);
                        command.Parameters.AddWithValue("@Code", province.Code);
                        command.ExecuteNonQuery();
                    }
                }
                using (var command = new SqlCommand("select code, id from Province;", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        provinceDictionary.Add((int)reader["code"], (int)reader["id"]);
                    }
                    reader.Close();
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
        #region [private]

        private static int CountTest(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("select count(*) from Home;", connection))
            {
                connection.Open();
                var count= command.ExecuteScalar();
            }
            return 1;
        }

        private static int SelectAllTest(string connectionString)
        {
            List<AdoHome> homes = new List<AdoHome>();
            AdoHome home;
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("select Id, Surface, Price, BuildYear, City, Description, AddTime, ProvinceId from Home;", connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    home = new AdoHome
                    {
                        BuildYear = (int)reader["BuildYear"],
                        City = (string)reader["City"],
                        Description = (string)reader["Description"],
                        Id = (int)reader["Id"],
                        Price = (double)(decimal)reader["Price"],
                        ProvinceId = (int)reader["ProvinceId"],
                        Surface = (int)reader["Surface"]
                    };
                    homes.Add(home);
                }
                reader.Close();
                connection.Close();
            }
            return homes.Count;
        }

        private static int SelectPartTest(string connectionString)
        {
            List<AdoHome> homes = new List<AdoHome>();
            AdoHome home;
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("select Id, Surface, Price, BuildYear, City, Description, AddTime, ProvinceId from Home where BuildYear<@BuildYear;", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@BuildYear", 2000);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    home = new AdoHome
                    {
                        BuildYear = (int)reader["BuildYear"],
                        City = (string)reader["City"],
                        Description = (string)reader["Description"],
                        Id = (int)reader["Id"],
                        Price = (double)(decimal)reader["Price"],
                        ProvinceId = (int)reader["ProvinceId"],
                        Surface = (int)reader["Surface"]
                    };
                    homes.Add(home);
                }
                reader.Close();
                connection.Close();
            }
            return homes.Count;
        }

        private static int SelectJoinTest(string connectionString)
        {
            List<AdoHome> homes = new List<AdoHome>();
            AdoHome home;
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(@"select h.Id, h.Surface, h.Price, h.BuildYear, h.City, 
h.Description, h.AddTime, h.ProvinceId, p.Name
from Home h
inner join Province p on h.ProvinceId=p.Id
where p.Code=@Code;", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Code", 10);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    home = new AdoHome
                    {
                        BuildYear = (int)reader["BuildYear"],
                        City = (string)reader["City"],
                        Description = (string)reader["Description"],
                        Id = (int)reader["Id"],
                        Price = (double)(decimal)reader["Price"],
                        ProvinceId = (int)reader["ProvinceId"],
                        Surface = (int)reader["Surface"]
                    };
                    homes.Add(home);
                }
                reader.Close();
                connection.Close();
            }
            return homes.Count;
        }

        private static int InsertTest(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                Dictionary<int, int> provinceDictionary = new Dictionary<int, int>();
                connection.Open();

                using (var command = new SqlCommand("select code, id from Province;", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        provinceDictionary.Add((int)reader["code"], (int)reader["id"]);
                    }
                    reader.Close();
                }

                foreach (var home in HomeData.Get100Homes())
                {
                    using (var command = new SqlCommand("insert into Home (Surface, Price, BuildYear, City, Description, AddTime, ProvinceId) values(@Surface, @Price, @BuildYear, @City, @Description, @AddTime, @ProvinceId);", connection))
                    {
                        command.Parameters.AddWithValue("@Surface", home.Surface);
                        command.Parameters.AddWithValue("@Price", home.Price);
                        command.Parameters.AddWithValue("@BuildYear", home.BuildYear);
                        command.Parameters.AddWithValue("@City", home.City);
                        command.Parameters.AddWithValue("@Description", home.Description);
                        command.Parameters.AddWithValue("@AddTime", DateTime.Now);
                        command.Parameters.AddWithValue("@ProvinceId", provinceDictionary[home.HomeProvince.Code]);
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
            return 100;
        }

        private static int UpdateTest(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(@"Update Home 
set BuildYear = @NewBuildYear
where BuildYear=@OldBuildYear", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@NewBuildYear", 2015);
                command.Parameters.AddWithValue("@OldBuildYear", 2014);
                command.ExecuteNonQuery();
                connection.Close();
            }
            return 100;
        }

        private static int DeleteTest(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(@"delete from Home 
where BuildYear=@BuildYear", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@BuildYear", 2015);
                command.ExecuteNonQuery();
                connection.Close();
            }
            return 100;
        }
        #endregion [private]
    }
}