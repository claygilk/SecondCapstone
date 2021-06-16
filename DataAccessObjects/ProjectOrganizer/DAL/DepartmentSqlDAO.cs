using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class DepartmentSqlDAO : IDepartmentDAO
    {
        public bool InTestMode { get; set; }

        private readonly string connectionString;

        // private string constants to store SQL commands
        private const string SqlGetDepartment = "SELECT * FROM department ORDER BY department_id;";
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
            // create List object that implements ICollection. To be returned later
            List<Department> departments = new List<Department>();

            try
            {
                // establish and open SQL connection
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlGetDepartment, conn);

                    // create command and sql reader to read the result rows
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // create Department object and set values from the rows returned by the data reader
                        Department dept = new Department();

                        dept.Id = Convert.ToInt32(reader["department_id"]);
                        dept.Name = Convert.ToString(reader["name"]);

                        // add the department to the List of departments
                        departments.Add(dept);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("No departments found: " + ex.Message);

                if (InTestMode)
                {
                    throw;
                }
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
                // establish and open SQL connection
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlCreateDepartment, conn);

                    // create command and pass in the name of the department (from the department object) as a parameter
                    command.Parameters.AddWithValue("@department_name", newDepartment.Name);

                    // capture the new ID of the department in the database
                    // this ID is captured by the 'SELECT @@identity' portion of the SQL command
                    newDepartment.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Couldn't create department: " + ex.Message);
                throw;
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
                // establish and open SQL connection
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // create command and pass in paramaters
                    SqlCommand command = new SqlCommand(SqlUpdateDepartment, conn);

                    // the parameters are the new name of the Department object and the ID (primary key) of the row in the database
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
