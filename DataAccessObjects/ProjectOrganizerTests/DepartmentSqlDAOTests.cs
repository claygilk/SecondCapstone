using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class DepartmentSqlDAOTests : UnitTestBase
    {
        [TestMethod]
        public void GetDepartments_ReturnsAllDepartments()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(this.ConnectionString);
            // Act
            ICollection<Department> actual =  dao.GetDepartments();

            int expectedCount = this.GetRowCount("department");
            // Assert
            Assert.AreEqual(expectedCount, actual.Count);

        }

        [TestMethod]
        public void CreateDepartments_ShouldIncreaseDepartmentCountBy1()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(this.ConnectionString);
            
            // Act
            Department dept = new Department();

            dept.Name = "Test Dept.";

            int oldRowCount = this.GetRowCount("department");

            int newID = dao.CreateDepartment(dept);

            // Assert
            Assert.AreEqual(oldRowCount + 1, this.GetRowCount("department"));

        }

        [TestMethod]
        public void UpdateDepartment_ShouldChangeNameOfDeparment()
        {
            // Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(this.ConnectionString);

            List<Department> depts = dao.GetDepartments().ToList();

            depts[0].Name = "Test new name";
            // Act

            bool success = dao.UpdateDepartment(depts[0]);

            depts = dao.GetDepartments().ToList();

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual("Test new name", depts[0].Name);
        }
    }
}
