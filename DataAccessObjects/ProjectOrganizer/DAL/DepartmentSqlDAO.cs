using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class DepartmentSqlDAO : IDepartmentDAO
    {
        private readonly string connectionString;

        private const string SqlGetDepartment = "SELECT * FROM department;";
        private const string SqlCreateDepartment = "INSERT INTO department (name) VALUES (@department_name); SELECT @@IDENTITY;";
        private const string SqlUpdateDepartment = "UPDATE department SET name = @newName WHERE department_id = @departmentId;";

        // Single Parameter Constructor
        public DepartmentSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns>Returns a List of Department objects</returns>
        public ICollection<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlGetDepartment, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Department dept = new Department();

                        dept.Id = Convert.ToInt32(reader["department_id"]);
                        dept.Name = Convert.ToString(reader["name"]);

                        departments.Add(dept);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("No departments found: " + ex.Message);
            }

            return departments;
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlCreateDepartment, conn);

                    command.Parameters.AddWithValue("@department_name", newDepartment.Name);

                    newDepartment.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Couldn't create department: " + ex.Message);
            }

            return newDepartment.Id;
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlUpdateDepartment, conn);

                    command.Parameters.AddWithValue("@newName", updatedDepartment.Name);
                    command.Parameters.AddWithValue("@departmentId", updatedDepartment.Id);

                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not update department: " + ex.Message);
            }

            return false;
        }
    }
}
