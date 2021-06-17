using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// This class handles working with Venues in the database.
    /// </summary>
    public class VenueDAO
    {
        private readonly string connectionString;

        private const string SqlGetAllVenues = "SELECT * FROM venue";

        public VenueDAO (string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Venue> GetAllVenues()
        {
            List<Venue> venues = new List<Venue>();

            return venues;
        }
    }
}
