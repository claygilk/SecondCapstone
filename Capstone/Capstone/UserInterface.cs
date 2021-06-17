using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    /// <summary>
    /// This class is responsible for representing the main user interface to the user.
    /// </summary>
    /// <remarks>
    /// ALL Console.ReadLine and WriteLine in this class
    /// NONE in any other class. 
    ///  
    /// The only exceptions to this are:
    /// 1. Error handling in catch blocks
    /// 2. Input helper methods in the CLIHelper.cs file
    /// 3. Things your instructor explicitly says are fine
    /// 
    /// No database calls should exist in classes outside of DAO objects
    /// </remarks>
    public class UserInterface
    {
        private readonly string connectionString;

        private readonly VenueDAO venueDAO;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;
            venueDAO = new VenueDAO(connectionString);
        }

        public void Run()
        {
            Console.WriteLine("Reached the User Interface.");

            this.MainMenu();

        }

        public void MainMenu()
        {
            // Asks user to select
            // 1 list venue
            // 2 quit
        }

        public void ViewVenu()
        {
            // Shows all venues to user
            // 1...
            // 2...
            // 3...
            // R - returns to previous screen
        }

        public void VenueDetails()
        {
            // Displays
            // Name
            // Location (City,State)
            // Categories

            // Description:

            // Prompt user to select
            // 1. View Spaces

            // 2. Search for Reservation - shows all spaces availble during a given chunk of time

            // r. return to prev screen
        }

        public void ListSpaces()
        {
            // Display all Space information: Name, open/close, rate, max occup

            // prompt user to reserve space

            // or return to previous screen
        }

        public void ReserveSpace()
        {
            // prompt for: start date

            // prompt for: duration

            // prompt for: attendees

            // Display all spaces that meet search criterea, if any

            // prompt user to select available Space

            // prompt user for a name to put on the reservation

            // displays details of reservation (newly created)

        }

        public void DisplayVenue(Venue venue)
        {
            // Display all venue info
        }

        public void DisplaySpaceInfo(Space space)
        {
            // display all space info
        }

        public void DisplaySpaceForReservation(Space space)
        {
            // display all space info
        }

        public void DisplayReservation(Reservation reservation)
        {
            // display all reservation info
        }
    }
}
