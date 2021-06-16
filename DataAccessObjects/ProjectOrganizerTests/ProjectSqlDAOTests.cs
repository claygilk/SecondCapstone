using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System.Data.SqlClient;

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
        [ExpectedException(typeof(SqlException))]
        public void AssignEmployeeToProject_ThrowsException_WhenProjectDoesntExist()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);
            dao.InTestMode = true;
           
            // Act
            bool success = dao.AssignEmployeeToProject(3,1);

            // Assert
            Assert.Fail("Expected a SQL Exception to be thrown");
        }

        [TestMethod]
        public void AssignEmployeeToProject_ReturnsTrue_WhenAddIsSuccessful()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);
            
            // Act
            bool success = dao.AssignEmployeeToProject(1,1);

            // Assert
            Assert.IsTrue(success);

        }

        [TestMethod]
        public void RemoveEmployeeFromProject_DeletesRowFrom_project_employeeTable()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);
            
            // Act
            bool success = dao.RemoveEmployeeFromProject(1, 2);
            
            // Assert
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void CreateProject_AddsRowTo_projectTable()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);

            Project proj = new Project();

            proj.Name = "Test Project";
            proj.StartDate = new DateTime(2001, 01, 01);
            proj.EndDate = new DateTime(2001, 01, 02);

            // Act
            int actualId = dao.CreateProject(proj);

            // Assert
            Assert.IsTrue(actualId > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void CreateProject_ThrowsException_WhenProjectAlreadyExists()
        {
            // Arrange
            ProjectSqlDAO dao = new ProjectSqlDAO(this.ConnectionString);
            dao.InTestMode = true;

            Project proj = new Project();

            proj.Name = "Forlorn TestCake";
            proj.StartDate = new DateTime(2001, 01, 01);
            proj.EndDate = new DateTime(2001, 01, 02);

            // Act
            dao.CreateProject(proj);

            // Assert
            Assert.Fail("Expected a SQL exception to be thrown");
        }
    }
}
