using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;

namespace ProjectOrganizer.DAL
{
    public class DepartmentSqlDAO : IDepartmentDAO
    {
        private readonly string connectionString;

        private const string SqlGetDepartment = "Select * FROM department;";
        private const string SqlCreateDepartment = "Insert Into department (name) VALUES @department_name;";
        private const string SqlUpdateDepartment = "Update department set (name) = @newName where name = @oldName;";

        // Single Parameter Constructor
        public DepartmentSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public ICollection<Department> GetDepartments()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            throw new NotImplementedException();
        }

    }
}
