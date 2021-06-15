using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace ProjectOrganizerTests
{
    [TestClass]
    public abstract class UnitTestBase
    {
        private TransactionScope transaction;

        protected string ConnectionString { get; } = "Server=.\\SQLEXPRESS;Database=EmployeeDB;Trusted_Connection=True;";


        [TestInitialize]
        public void Setup()
        {
            transaction = new TransactionScope(); // BEGIN TRANSACTION

            string sql = File.ReadAllText("Setup.sql");

            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose(); // ROLLBACK TRANSACTION
        }

        protected int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count;
            }
        }
    }
}
