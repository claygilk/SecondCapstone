using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SpaceDAO
    {
        private readonly string connectionString;

        private const string SqlGetAllSpaces = "SELECT * FROM space WHERE venue_id = @venueId";
        private const string SqlSearchSpaceAvailability = "SELECT * FROM space WHERE open_from < @startDate AND open_to > @endDate;";
        private const string SqlSearchSpaceAvailabilityTop5 = "SELECT TOP 5 * FROM space WHERE open_from < @startDate AND open_to > @endDate;";

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

            return spaces;
        }

        /// <summary>
        /// Searches the avaibility of all spaces at a given venue within a certain time frame
        /// </summary>
        /// <param name="venueId"></param>
        /// <param name="startDate"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public List<Space> SearchSpaceAvailability(int venueId, DateTime startDate, int numberOfDays)
        {
            List<Space> spaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlSearchSpaceAvailability , conn);
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return spaces;
        }

        /// <summary>
        /// returns the top 5 avaiable spaces at a given venue within a certain time frame
        /// </summary>
        /// <param name="venueId"></param>
        /// <param name="startDate"></param>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        public List<Space> SearchTop5SpaceAvailability(int venueId, DateTime startDate, int numberOfDays)
        {
            List<Space> spaces = new List<Space>();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SqlSearchSpaceAvailabilityTop5, conn);
                }
            }
            catch (SqlException)
            {

                throw;
            }

            return spaces;
        }
    }
}
