using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ORMPerformanceTest.Tests.Bulk
{
    public class BulkUploadToSql
    {
        private List<Home> internalStore;

        protected string tableName;
        protected string _connectionString;
        protected DataTable dataTable = new DataTable();
        protected int recordCount;
        protected int commitBatchSize;
        private BulkUploadToSql(
       string tableName,
       int commitBatchSize, string connectionString)
        {
            internalStore = new List<Home>();

            this.tableName = tableName;
            this.dataTable = new DataTable(tableName);
            this.recordCount = 0;
            this.commitBatchSize = commitBatchSize;
            _connectionString = connectionString;

            
            InitializeStructures();
        }

        private void InitializeStructures()
        {
            this.dataTable.Columns.Add("Id", typeof(Int32));
            this.dataTable.Columns.Add("Surface", typeof(Int32));
            this.dataTable.Columns.Add("Price", typeof(decimal));
            this.dataTable.Columns.Add("BuildYear", typeof (Int32));
            this.dataTable.Columns.Add("City", typeof (string));
            this.dataTable.Columns.Add("Description", typeof (string));
            this.dataTable.Columns.Add("ProvinceId", typeof (Int32));
            this.dataTable.Columns.Add("AddTime", typeof (DateTime));
        }
        public static BulkUploadToSql Load(IEnumerable<Home> dataSource, string tableName,
       int commitBatchSize, string connectionString)
        {
            BulkUploadToSql o = new BulkUploadToSql(tableName,commitBatchSize,connectionString);

            foreach (var home in dataSource)
            {
                o.internalStore.Add(home);
            }

            return o;
        }
        public void Flush()
        {
            foreach (Home rec in this.internalStore)
            {
                this.PopulateDataTable(rec);
                if (this.recordCount >= this.commitBatchSize)
                    this.WriteToDatabase();
            }
            if (this.recordCount > 0)
                this.WriteToDatabase();
        }

        private void PopulateDataTable(Home record)
        {
            DataRow row;
            row = this.dataTable.NewRow();

            row[1] = record.Surface;
            row[2] = record.Price;
            row[3] = record.BuildYear;
            row[4] = record.City;
            row[5] = record.Description;
            row[6] = record.ProvinceId;
            row[7] = record.AddTime;

            this.dataTable.Rows.Add(row);
            this.recordCount++;
        }
        private void WriteToDatabase()
        {
            string connString = _connectionString;
            using (SqlConnection connection =
                    new SqlConnection(connString))
            {
                SqlBulkCopy bulkCopy =
                    new SqlBulkCopy
                    (
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                    );

                bulkCopy.DestinationTableName = this.tableName;
                connection.Open();

                bulkCopy.WriteToServer(dataTable);
                connection.Close();
            }
            this.dataTable.Clear();
            this.recordCount = 0;
        }
    }
}