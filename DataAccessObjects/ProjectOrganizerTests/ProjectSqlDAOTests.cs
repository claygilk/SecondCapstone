using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class ProjectSqlDAOTests : UnitTestBase
    {
        [TestMethod]
        public void GetAllProject_ReturnsAllProjects()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);
            // Act
            ICollection<Project> actual = dao.GetAllProjects();

            int expected = GetRowCount("project");
            // Assert
            Assert.AreEqual(expected, actual.Count);
        }

        [TestMethod]
        public void AssignEmployeeToProject_AddsRowTo_project_employeeTable()
        {
            // Arrange

            // Act

            // Assert
        }

        [TestMethod]
        public void RemoveEmployeeFromProject_DeletesRowFrom_project_employeeTable()
        {
            // Arrange

            // Act

            // Assert
        }

        [TestMethod]
        public void CreateProject_AddsRowTo_projectTable()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
