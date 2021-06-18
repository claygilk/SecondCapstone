using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public int MakeReservation(Reservation newReservation)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(this.connectionString)) 
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlMakeReservation, conn);

                    Reservation reservation = new Reservation();
                    command.Parameters.AddWithValue("@spaceId", newReservation.SpaceID);
                    command.Parameters.AddWithValue("@attendees", newReservation.NumberOfAttendes);
                    command.Parameters.AddWithValue("@StartDate", newReservation.StartDate);
                    command.Parameters.AddWithValue("@EndDate", newReservation.EndDate);
                    command.Parameters.AddWithValue("@customerName", newReservation.ReservedFor);

                    newReservation.ReservationID = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine("could not make reservation"); ;
            }
            return newReservation.ReservationID;
        }

        public Reservation GetLastReservation(int reservationID)
        {
            Reservation reservation = new Reservation();
            try
            {
                using(SqlConnection conn = new SqlConnection(this.connectionString)) 
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlGetLastReservation);
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine("No record of that reservation"); ;
            }
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
