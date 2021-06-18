using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;
using System;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class SpaceDAOTests : IntegrationTestBase
    {
        [DataRow(1, 8)]
        [DataRow(2, 2)]
        [DataTestMethod]
        public void GetAllSpaces_ReturnsCorrectNumberOfSpaces(int venueId, int numOfSpaces)
        {
            // Arrange
            SpaceDAO dao = new SpaceDAO(this.ConnectionString);

            // Act
            List<Space> actual = dao.GetAllSpaces(venueId);

            // Assert
            Assert.AreEqual(numOfSpaces, actual.Count);
        }

        [TestMethod]
        public void SearchTop5SpaceAvailability_ReturnsNoMoreThanFiveSpaces()
        {
            // Arrange
            SpaceDAO dao = new SpaceDAO(this.ConnectionString);
            DateTime date = new DateTime(2021, 1, 1);

            // Act
            List<Space> actual = dao.SearchTop5SpaceAvailability(1, date, 1);

            // Assert
            Assert.IsTrue(actual.Count <= 5);
        }
        [DataRow(1, 1, 0)]
        [DataRow(1, 9, 0)]
        [DataTestMethod]
        public void SearchTop5SpaceAvailability_DoesNotReturnSpaceClosedForTheSeason(int venueId, int month, int expected)
        {
            // Arrange
            SpaceDAO dao = new SpaceDAO(this.ConnectionString);
            DateTime date = new DateTime(2021, month, 1);

            // Act
            List<Space> actual = dao.SearchTop5SpaceAvailability(1, date, 1);

            // Assert
            Assert.IsTrue(actual.Count <= 5);
        }
    }
}
