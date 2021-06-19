using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class IntegrationTestBaseTests : IntegrationTestBase
    {
        [DataRow("state", 2)]
        [DataRow("city", 2)]
        [DataRow("venue", 3)]
        [DataRow("space", 11)]
        [DataRow("category", 2)]
        [DataRow("category_venue", 3)]
        [DataRow("reservation", 5)]
        [DataTestMethod]
        public void GetRowCount_ReturnsCorrectNumberOfRows(string table, int expectedRows)
        {
            // Arrange
            // Act
            int actual = this.GetRowCount(table);

            // Assert
            Assert.AreEqual(expectedRows, actual);
        }
    }
}
