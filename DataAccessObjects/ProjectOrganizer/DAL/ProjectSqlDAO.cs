using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{
    public class ProjectSqlDAO : IProjectDAO
    {
        private readonly string connectionString;

        private const string SqlGetAllProjects = "Select * From project;";
        private const string SqlAssignEmployee = "Insert Into project_employee(project_id, employee_id) Values(@projectId, @employeeId);";
        private const string SqlRemoveEmployee = "Delete From project_employee Where employee_id = @employeeId;";
        private const string SqlCreateProject = "Insert Into project(name, from_date, to_date) Values(@projectName, @startDate, @endDate); Select @@IDENTITY";
        // Single Parameter Constructor
        public ProjectSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public ICollection<Project> GetAllProjects()
        {
            List<Project> projects = new List<Project>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(SqlGetAllProjects, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Project proj = new Project();
                        proj.ProjectId = Convert.ToInt32(reader["project_id"]);
                        proj.Name = Convert.ToString(reader["name"]);
                        proj.StartDate = Convert.ToDateTime(reader["from_date"]);
                        proj.EndDate = Convert.ToDateTime(reader["to_date"]);

                        projects.Add(proj);


                    }
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine("could not find projects"); ;
            }
            return projects;
        }

        /// <summary>
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))

                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlAssignEmployee, conn);
                    command.Parameters.AddWithValue("@projectId", projectId);
                    command.Parameters.AddWithValue("@employeeId", employeeId);

                    command.ExecuteNonQuery();
                }



                return true;
            }
            catch (SqlException ex)
            {

                return false;
            }
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))

                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlRemoveEmployee, conn);
                    command.Parameters.AddWithValue("@projectId", projectId);
                    command.Parameters.AddWithValue("@employeeId", employeeId);

                    command.ExecuteNonQuery();
                }



                return true;
            }
            catch (SqlException ex)
            {

                return false;
            }
        }
        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString)) 
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlCreateProject, conn);
                    command.Parameters.AddWithValue("@projectName", newProject.Name);
                    command.Parameters.AddWithValue("@startDate", newProject.StartDate);
                    command.Parameters.AddWithValue("@endDate", newProject.EndDate);

                    

                    newProject.ProjectId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine("could not create project"); ;
            }
            return newProject.ProjectId;
        }   
    }



}

