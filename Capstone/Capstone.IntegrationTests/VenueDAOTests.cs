using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class VenueDAOTests : IntegrationTestBase
    {
        [TestMethod]
        public void GetAllVenues_ReturnsCorrectNumberOfVenues()
        {
            // Arrange
            VenueDAO dao = new VenueDAO(this.ConnectionString);

            int expected = this.GetRowCount("venue");

            // Act
            List<Venue> actual = dao.GetAllVenues();

            // Assert
            Assert.AreEqual(expected, actual.Count);

        }

        [DataRow(1, "Hidden Owl Eatery")]
        [DataRow(2, "Painted Squirrel Club")]
        [DataTestMethod]
        public void SelectVenues_ReturnsOnly_TheCorrectVenue(int venueId, string expectedName)
        {
            // Arrange
            VenueDAO dao = new VenueDAO(this.ConnectionString);
            
            // Act
            Venue actual = dao.SelectVenues(venueId);

            // Assert
            Assert.AreEqual(expectedName, actual.Name);
        }

        [DataRow(1, 2)]
        [DataRow(2, 1)]
        [DataTestMethod]
        public void SelectCategoriesForVenue_ReturnsCorrectNumberOfCategories(int venueId, int expectedNumOfCategories)
        {
            // Arrange
            VenueDAO dao = new VenueDAO(this.ConnectionString);

            // Act
            List<string> actual = dao.GetCategoriesForVenue(venueId);

            // Assert
            Assert.AreEqual(expectedNumOfCategories, actual.Count);
        }
    }
}
