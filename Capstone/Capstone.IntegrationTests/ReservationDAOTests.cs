using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.IntegrationTests
{
    [TestClass]
    public class ReservationDAOTests : IntegrationTestBase
    {
        [TestMethod]
        public void MakeReservation_AddsRowToReservationTable()
        {
            // Arrange
            Reservation reservation = new Reservation();
            reservation.SpaceID = 1;
            reservation.NumberOfAttendes = 10;
            reservation.StartDate = new DateTime(2021, 01, 01);
            reservation.EndDate = reservation.StartDate.AddDays(5);
            reservation.ReservedFor = "Test Client";

            ReservationDAO dao = new ReservationDAO(this.ConnectionString);
            // Act
            int newId = dao.MakeReservation(reservation);

            // Assert
            // There are 5 reservations in the database at the start of the transaction
            // So the next one to be add should be assigned the id of 6
            Assert.AreEqual(6, newId);
        }

        [TestMethod]
        public void ValidParySize_ReturnsFalseWhen_AttendeesExceedsMaxOccup()
        {
            // Arrange
            Reservation reservation = new Reservation();
            reservation.SpaceID = 1;
            reservation.NumberOfAttendes = 1000;
            reservation.StartDate = new DateTime(2021, 01, 01);
            reservation.EndDate = reservation.StartDate.AddDays(5);
            reservation.ReservedFor = "Test Client";

            ReservationDAO dao = new ReservationDAO(this.ConnectionString);
            // Act
            int newId = dao.MakeReservation(reservation);

            // Assert
            // If no resevation is created MakeReservation() will return default value '0'
            Assert.AreEqual(0, newId);
        }
    }
}
