using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueDAO
    {
        private readonly string connectionString;

        private const string SqlSelectVenues =
            "SELECT v.id, v.name, v.description, c.name AS city, s.name AS state " +
            "FROM venue v " +
            "JOIN city c on v.city_id = c.id " +
            "JOIN state s on c.state_abbreviation = s.abbreviation " +
            "WHERE v.id = @venueID " +
            "GROUP BY v.id, v.name, v.description, c.name, s.name";

        private const string SqlGetAllVenues =
            "SELECT v.id, v.name, v.description, c.name AS city, s.name AS state " +
            "FROM venue v " +
            "JOIN city c on v.city_id = c.id " +
            "JOIN state s on c.state_abbreviation = s.abbreviation " +
            "GROUP BY v.id, v.name, v.description, c.name, s.name";

        private const string SqlGetcategories =
            "SELECT c.name from category c " +
            "JOIN category_venue cv ON c.id = cv.category_id " +
            "JOIN venue v ON v.id = cv.venue_id " +
            "WHERE v.id = @venueId";
        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Venue> GetAllVenues()
        {
            List<Venue> venues = new List<Venue>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(SqlGetAllVenues, conn);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Venue venueDetails = new Venue();

                        venueDetails.Id = Convert.ToInt32(reader["id"]);
                        venueDetails.Name = Convert.ToString(reader["name"]);
                        venueDetails.City = Convert.ToString(reader["city"]);
                        venueDetails.State = Convert.ToString(reader["state"]);
                        venueDetails.Description = Convert.ToString(reader["description"]);
                        venues.Add(venueDetails);
                    }
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine("Could not find Venues"); ;
            }
            return venues;
        }

        public Venue SelectVenues(int venueID)
        {
            Venue venue = new Venue();

            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(SqlSelectVenues, conn);
                    command.Parameters.AddWithValue("@venueID", venueID);

                    SqlDataReader reader = command.ExecuteReader();
                   
                    while (reader.Read())
                    {
                        venue.Id = Convert.ToInt32(reader["id"]);
                        venue.Name = Convert.ToString(reader["name"]);
                        venue.City = Convert.ToString(reader["city"]);
                        venue.State = Convert.ToString(reader["state"]);
                        venue.Description = Convert.ToString(reader["description"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return venue;
        }
        public List<string> GetCategoriesForVenue()
        {
            List<string> categories = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(SqlGetcategories);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        categories.Add(Convert.ToString(reader["name"]));
                    }
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine("Could not get categories" + ex.Message);
            }
            return categories;
        }

    }
}
