using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private readonly string connectionString;

        private const string SqlGetAllEmployees = "Select * FROM employee;";
        private const string SqlSearchEmployee = "Select * FROM employee WHERE first_name LIKE @firstname AND last_name LIKE @lastname;";
        private const string SqlGetEmployeeWithoutProject = "SELECT * FROM employee e LEFT JOIN project_employee P ON p.employee_id = e.employee_id WHERE project_id IS NULL;";
        
        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Employee> SelectFromEmployee(string sqlStatement)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sqlStatement, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

                        employees.Add(employee);
                    }
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not find employees: " + ex.Message);
            }
            return employees;
        }
        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public ICollection<Employee> GetAllEmployees()
        {
            return SelectFromEmployee(SqlGetAllEmployees);
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            

        /// <summary>
        /// Find all employees whose names contain the search strings.
        /// Returned employees names must contain *both* first and last names.
        /// </summary>
        /// <remarks>Be sure to use LIKE for proper search matching.</remarks>
        /// <param name="firstname">The string to search for in the first_name field</param>
        /// <param name="lastname">The string to search for in the last_name field</param>
        /// <returns>A list of employees that matches the search.</returns>
        public ICollection<Employee> Search(string firstname, string lastname)
        {
            List<Employee> employees = new List<Employee>();

            try 
            { 
                using (SqlConnection conn = new SqlConnection(this.connectionString)) 
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSearchEmployee, conn);

                    command.Parameters.AddWithValue("@firstname", firstname);
                    command.Parameters.AddWithValue("@lastname", lastname);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read()) 
                    {
                        Employee employee = new Employee();
                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

                        employees.Add(employee);
                    }

                }

            
            }
            catch (SqlException ex) 
            {
                Console.WriteLine("Could not find employees: " + ex.Message);
            }
            return employees;
        }   

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Employee> GetEmployeesWithoutProjects()
        {
            return SelectFromEmployee(SqlGetEmployeeWithoutProject);
        }

    }
}
