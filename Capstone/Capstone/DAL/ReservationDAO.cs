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

        private const string SqlGetMaxOccupancy = "SELECT max_occupancy FROM space s WHERE s.id = @spaceId";

        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Adds row to reservation table with attributes provided by the Reservation object
        /// </summary>
        /// <param name="newReservation"></param>
        /// <returns>SQL query uses SELECT @@IDENTITY to retur id of new reservation. If the reservation is
        /// unsuccesfull the method returns '0' </returns>
        public int MakeReservation(Reservation newReservation)
        {
            try
            {
                // This method checks to make sure the party size does not exceed the max occup
                // This is a fail safe in case the SpaceDAO class fails to filter out spaces that are not large enough
                if (ValidPartySize(newReservation))
                {
                    using (SqlConnection conn = new SqlConnection(this.connectionString))
                    {
                        conn.Open();

                        SqlCommand command = new SqlCommand(SqlMakeReservation, conn);

                        command.Parameters.AddWithValue("@spaceId", newReservation.SpaceID);
                        command.Parameters.AddWithValue("@attendees", newReservation.NumberOfAttendes);
                        command.Parameters.AddWithValue("@StartDate", newReservation.StartDate);
                        command.Parameters.AddWithValue("@EndDate", newReservation.EndDate);
                        command.Parameters.AddWithValue("@customerName", newReservation.ReservedFor);

                        newReservation.ReservationID = Convert.ToInt32(command.ExecuteScalar());
                    }
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine("could not make reservation: " + ex.Message); ;
            }
            return newReservation.ReservationID;
        }

        /// <summary>
        /// This method checks to make sure the the party size of the reservatoin does not excced the max occup of the space.
        /// </summary>
        /// <param name="newReservation"></param>
        /// <returns>returns true if the space can accomadate the number of occupants</returns>
        public bool ValidPartySize(Reservation newReservation)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    // queries database to get max occup for the space the user is trying to reserve
                    SqlCommand command = new SqlCommand(SqlGetMaxOccupancy, conn);
                    command.Parameters.AddWithValue("@spaceId", newReservation.SpaceID);

                    int maxOccupancy = Convert.ToInt32(command.ExecuteScalar());

                    if (maxOccupancy >= newReservation.NumberOfAttendes)
                    {
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("could not make reservation: " + ex.Message); ;
            }
            return false;
        }

        // BONUS
        public int CancelReservation()
        {
            // delete row from reservation table
            return 0;
        }

    }
}