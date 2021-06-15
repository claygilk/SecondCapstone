using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private readonly string connectionString;

        private const string SqlGetAllEmployees = "Select * From employee;";
        private const string SqlSearchEmployee = "Select * From employee Where firstname Like '%@firstname%' AND lastname Like '%@lastname%';";
        private const string SqlGetEmployeeWithoutProject = "Select * FROM employee left join project_employee e ON project p.employee_id = e.employee_id Where project_id IS NULL;";
        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public ICollection<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            try 
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString)) 
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(SqlGetAllEmployees, conn);

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
            catch(SqlException ex) 
            {
                Console.WriteLine("Could not get emploees");
            }
            return employees;
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
                Console.WriteLine("could not find employee by that name");
            }
            return employees;
        }   

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(SqlGetEmployeeWithoutProject, conn);

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
                Console.WriteLine("Could not get emploees");
            }
            return employees;
        }

    }
}
