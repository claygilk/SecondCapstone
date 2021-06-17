using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO
    {
        private readonly string connectionString;

        private const string SqlMakeReservation = "INSERT INTO reservation " +
            "(space_id, number_of_attendees, start_date, end_date, reserved_for) " +
            "VALUES(@spaceId, @attendees, @startDate, @endDate, @customerName); " +
            "SELECT @@IDENTITY;";

        private const string SqlGetLastReservation = "SELECT * FROM reservation WHERE reservation_id = @reservationId";

        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Adds row to reservatoin table
        // returns id of new reservation
        // SELECT @@IDENTITY
        public int MakeReservation()
        {
            return 0;
        }

        public Reservation GetLastReservation()
        {
            Reservation reservation = new Reservation();

            return reservation;
        }

        // BONUS
        public int CancelReservation()
        {
            // delete row from reservation table
            return 0;
        }

    }
}
