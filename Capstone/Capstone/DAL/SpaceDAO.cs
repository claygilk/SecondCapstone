using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone;

namespace Capstone.DAL
{
    public class SpaceDAO
    {
        private readonly string connectionString;

        private const string SqlGetAllSpaces = "SELECT * FROM space WHERE venue_id = @venueId ";
        private const string SqlSearchSpaceAvailability = "SELECT * FROM space WHERE open_from < @startDate AND open_to > @endDate;";
        private const string SqlSearchSpaceAvailabilityTop5 =
            "SELECT TOP 5 s.id AS id, s.name AS name, daily_rate, max_occupancy, is_accessible " +
            "FROM space s " +
            "JOIN venue v ON v.id = s.venue_id " +
            "WHERE s.venue_id = @venueId " +
            "AND s.id NOT IN " +
            "(SELECT s.id FROM space s WHERE @startMonth < s.open_from OR @endMonth >= s.open_to) " +
            "AND s.id NOT IN " +
            "( SELECT s.id FROM space s JOIN reservation r ON r.space_id = s.id " +
            "WHERE @startDate BETWEEN r.start_date AND r.end_date " +
            "OR @endDate BETWEEN r.start_date AND r.end_date) " +
            "AND s.max_occupancy > @numOfAttendees " +
            "GROUP BY s.id,s.name, daily_rate, max_occupancy, is_accessible " +
            "ORDER BY daily_rate";

        public SpaceDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Returns a list of a spaces at a given venue
        /// </summary>
        /// <param name="venueId">the id of the venue the user is currently viewing</param>
        /// <returns>list of spaces at venue</returns>
        public List<Space> GetAllSpaces(int venueId)
        {
            List<Space> spaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlGetAllSpaces, conn);
                    command.Parameters.AddWithValue("@venueId", venueId);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Space spaceDetails = new Space();

                        spaceDetails.Id = Convert.ToInt32(reader["id"]);
                        spaceDetails.Name = Convert.ToString(reader["name"]);
                        spaceDetails.IsAccessible = Convert.ToBoolean(reader["is_accessible"]);
                        spaceDetails.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        spaceDetails.DailyRate = Convert.ToDecimal(reader["daily_rate"]);

                        // These if blocks only try to read the value from the database if it is not NULL
                        if (!(reader["open_from"] is DBNull))
                        {
                            spaceDetails.OpenFrom = CLIHelper.FirstOf(Convert.ToInt32(reader["open_from"]));
                        }

                        if (!(reader["open_to"] is DBNull))
                        {
                            spaceDetails.OpenTo = CLIHelper.LastOf(Convert.ToInt32(reader["open_to"]));
                        }

                        // add the space to the list to be returned
                        spaces.Add(spaceDetails);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("could not find spaces" + ex.Message);
            }
            return spaces;
        }

        /// <summary>
        /// returns the top 5 avaiable spaces at a given venue within a certain timeframe.
        /// The SQL query only returns spaces that are open for the season,
        /// not already reserved, and whose max occup is larger than the party size
        /// </summary>
        /// <param name="venueId"></param>
        /// <param name="startDate"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public List<Space> SearchTop5SpaceAvailability(int venueId, Reservation tempReservation, int numberOfDays)
        {
            List<Space> spaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlSearchSpaceAvailabilityTop5, conn);
                    cmd.Parameters.AddWithValue("@startDate", tempReservation.StartDate);
                    cmd.Parameters.AddWithValue("@startMonth", tempReservation.StartDate.Month);
                    cmd.Parameters.AddWithValue("@endDate", tempReservation.StartDate.AddDays(numberOfDays));
                    cmd.Parameters.AddWithValue("@endMonth", tempReservation.StartDate.AddDays(numberOfDays).Month);
                    cmd.Parameters.AddWithValue("@venueId", venueId);
                    cmd.Parameters.AddWithValue("@numOfAttendees", tempReservation.NumberOfAttendes);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Space space = new Space();
                        space.Id = Convert.ToInt32(reader["id"]);
                        space.Name = Convert.ToString(reader["name"]);
                        space.DailyRate = Convert.ToDecimal(reader["daily_rate"]);
                        space.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        space.IsAccessible = Convert.ToBoolean(reader["is_accessible"]);
                        space.DaysReserved = numberOfDays;

                        spaces.Add(space);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("could not find spaces: " + e.Message);
            }
            return spaces;
        }
    }
}
