using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectOrganizer.DAL
{

    public class ProjectSqlDAO : IProjectDAO
    {
        public bool InTestMode { get; set; }

        private readonly string connectionString;

        // Constants to store SQL queries/commands
        private const string SqlGetAllProjects = "SELECT * FROM project;";
        private const string SqlAssignEmployee = "INSERT INTO project_employee(project_id, employee_id) VALUES(@projectId, @employeeId);";
        private const string SqlRemoveEmployee = "DELETE FROM project_employee WHERE employee_id = @employeeId;";
        private const string SqlCreateProject = "INSERT INTO project(name, from_date, to_date) VALUES(@projectName, @startDate, @endDate); SELECT @@IDENTITY";

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
            // create list to store all Project objects that will be returned
            List<Project> projects = new List<Project>();
            try
            {
                // establish and open SQL connection
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // create SQL command and pass in the constnat string that stores the command
                    SqlCommand command = new SqlCommand(SqlGetAllProjects, conn);

                    // execute the command and create data reader
                    SqlDataReader reader = command.ExecuteReader();

                    // read through all the rows returned
                    while (reader.Read())
                    {
                        // create new Project object for each row in the resutl table
                        Project proj = new Project();

                        // assign the values from each column to the Project object
                        proj.ProjectId = Convert.ToInt32(reader["project_id"]);
                        proj.Name = Convert.ToString(reader["name"]);
                        proj.StartDate = Convert.ToDateTime(reader["from_date"]);
                        proj.EndDate = Convert.ToDateTime(reader["to_date"]);

                        // add the new Project to the list of projects
                        projects.Add(proj);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("could not find projects"); ;
            }
            // return the list of projects just created
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
                // establish and open SQL connection
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // create SQL command and pass in the constnat string that stores the command
                    SqlCommand command = new SqlCommand(SqlAssignEmployee, conn);

                    // pass in the the employeeID and projectID of the employee being assinged and the project they are being assigned to
                    command.Parameters.AddWithValue("@projectId", projectId);
                    command.Parameters.AddWithValue("@employeeId", employeeId);

                    // execute SQL command
                    command.ExecuteNonQuery();
                }
                // returns true if the INSERT was succesful
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not assign employee: " + ex.Message);

                if (InTestMode)
                {
                    throw;
                }
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
                // establish and open SQL connection
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // build SQL command from constant string
                    SqlCommand command = new SqlCommand(SqlRemoveEmployee, conn);

                    // pass in the parameters: the employeeID of the employee being removed
                    // and the project ID of the project they are being taken off of
                    command.Parameters.AddWithValue("@projectId", projectId);
                    command.Parameters.AddWithValue("@employeeId", employeeId);

                    // execute SQL command
                    command.ExecuteNonQuery();
                }
                // method returns true if the DELETE was succesful
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not remove employee from project: " + ex.Message);

                // method returns false if the DELETE was unsuccesful
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
                // establish and open SQL connection
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // build SQL INSERT command
                    SqlCommand command = new SqlCommand(SqlCreateProject, conn);

                    // pass in attributes for the new row in the project table
                    command.Parameters.AddWithValue("@projectName", newProject.Name);
                    command.Parameters.AddWithValue("@startDate", newProject.StartDate);
                    command.Parameters.AddWithValue("@endDate", newProject.EndDate);

                    // execute command and use @@identity to grab the ID of the new row (if it was created)
                    // then ID is then assigned to the '.ProjectId' of the newProject object
                    newProject.ProjectId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("could not create project: " + ex); 

                if(InTestMode)
                {
                    throw;
                }
            }
            // Return the ID of the new project if one was created
            return newProject.ProjectId;
        }
    }
}

