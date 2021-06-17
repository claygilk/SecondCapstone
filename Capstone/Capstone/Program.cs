using Capstone.DAL;
using Capstone.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {


            #region call to appsettings.json for configuration
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            #endregion

            string connectionString = configuration.GetConnectionString("Project");


            ReservationDAO reservationDAO = new ReservationDAO(connectionString);
            SpaceDAO spaceDAO = new SpaceDAO(connectionString);
            VenueDAO venue = new VenueDAO(connectionString);

            UserInterface ui = new UserInterface(connectionString);


            ui.Run();

        }
    }
}
