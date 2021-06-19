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
        [DataRow(3, 1)]
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
        // venueId, month, spaces available
        [DataRow(2, 1, 0)]
        [DataRow(2, 9, 0)]
        [DataTestMethod]
        public void SearchTop5SpaceAvailability_DoesNotReturnSpaceClosedForTheSeason(int venueId, int month, int expected)
        {
            // Arrange
            SpaceDAO dao = new SpaceDAO(this.ConnectionString);
            DateTime date = new DateTime(2021, month, 1);

            // Act
            List<Space> actual = dao.SearchTop5SpaceAvailability(venueId, date, 1);

            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void SearchTop5SpaceAvailability_DoesNotDoubleBook()
        {
            // Arrange
            SpaceDAO dao = new SpaceDAO(this.ConnectionString);
            DateTime date = new DateTime(2021, 6, 1);

            // Act  
            // Venue 3 has one space (#7) and it is booked for all of 2021
            // So spaces should be returned
            List<Space> actual = dao.SearchTop5SpaceAvailability(3, date, 1);

            // Assert
            Assert.AreEqual(0, actual.Count);
        }
    }
}
