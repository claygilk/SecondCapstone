using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectOrganizer.Models;
using ProjectOrganizer.DAL;
using System.Linq;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class EmployeeSqlDAOTests : UnitTestBase
    {
        [TestMethod]
        public void GetAllEmployees_ReturnsAllEmployees()
        {
            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(this.ConnectionString);
            // Act
            ICollection<Employee> employees = dao.GetAllEmployees();
            int expected = this.GetRowCount("employee");
            // Assert
            Assert.AreEqual(expected, employees.Count);
        }

        [TestMethod]
        public void SqlGetEmployeeWithoutProject_ReturnCorrectEmployees()
        {
            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(this.ConnectionString);
            // Act
            ICollection<Employee> employees = dao.GetEmployeesWithoutProjects();
            int actual = employees.Count;
            // Assert
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void Search_ReturnsExpectedEmployee_IfEmployeeExists()
        {
            // Arrange
            EmployeeSqlDAO dao = new EmployeeSqlDAO(this.ConnectionString);
            // Act
            List<Employee> employees = dao.Search("Test", "Keppard").ToList();

            // Assert
            Assert.AreEqual("Test", employees[0].FirstName);
            Assert.AreEqual("Keppard", employees[0].LastName);
        }

        [DataTestMethod]
        [DataRow("Select * From employee Where job_title = 'Chief Head Honcho' ", "Test")]
        [DataRow("Select * From employee Where job_title = 'Floss Replenisher'", "Flo")]
        [DataRow("Select * From employee Where department_id = 2", "Flo")]
        public void SelectFromEmployee_ReturnsEmployees_ThatMeetWhereStatement(string sqlCommand, string expected)
        {
            // Arrange
           EmployeeSqlDAO dao = new EmployeeSqlDAO(this.ConnectionString);
            // Act
            List<Employee> employee = dao.SelectFromEmployee(sqlCommand);
            // Assert
            Assert.AreEqual(expected, employee[0].FirstName);
        }
    }
}
