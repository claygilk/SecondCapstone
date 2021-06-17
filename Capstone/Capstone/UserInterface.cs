using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        // Private Fields
        private readonly VenueDAO venueDAO;
        private readonly SpaceDAO spaceDAO;
        private readonly ReservationDAO reservationDAO;

        // Constructor
        public UserInterface(string connectionString)
        {
            venueDAO = new VenueDAO(connectionString);
            spaceDAO = new SpaceDAO(connectionString);
            reservationDAO = new ReservationDAO(connectionString);
        }

        // Methods
        public void MainMenu()
        {
            bool keepRunning = true;

            while (keepRunning)
            {
                // Asks user to select from two options:
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1) List Venues");
                Console.WriteLine("Q) Quit");

                string choice = Console.ReadLine();

                switch (choice.ToLower())
                {
                    // 1. list all venues
                    case "1":
                        ViewVenue();
                        break;

                    // 2. quit the program
                    case "q":
                        Console.WriteLine("Thank you for booking with Excelsior Venues!");
                        keepRunning = false;
                        return;
                }
            }
        }

        public void ViewVenue()
        {
            // Shows all venues to user
            Console.WriteLine("Which venue would you like to view?");

            List<Venue> venues = this.venueDAO.GetAllVenues();

            foreach (Venue v in venues)
            {
                Console.WriteLine($"{v.Id}) {v.Name}");
            }
            Console.WriteLine();

            string choice = Console.ReadLine();

            // R - returns to previous screen
            if (choice.ToLower() == "r")
            {
                return;
            }
            else
            {
                VenueDetails(Convert.ToInt32(choice));
            }

        }

        public void VenueDetails(int venueId)
        {
            Venue venue = venueDAO.SelectVenues(venueId);

            // Displays
            // Name
            // Location (City,State)
            // Categories
            // Description:
            DisplayVenue(venue);


            // Prompt user to select an option
            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1) View Spaces");
            //Console.WriteLine("2) Search for availble reservation slots");
            Console.WriteLine("R) Return to previous menu");

            string choice = Console.ReadLine();

            switch (choice.ToLower())
            {
                // 1. List all spaces at the current venue
                case "1":
                    ListSpaces(venueId);
                    break;

                //// 2. Search for Reservation - shows all spaces availble during a given chunk of time
                //case "2":
                //    SearchForReservation(venueId);
                //    break;

                // r. return to prev screen
                case "r":
                    return;
            }
        }

        //BONUS
        //public void SearchForReservation(int venueId)
        //{
        //    // prompt for: start date

        //    // prompt for: duration

        //    // Display top 5 spaces that meet search criterea, if any

        //    // Prompt:
        //    // Search Again
        //    // Return to previous menu
        //}

        public void ListSpaces(int venueId)
        {
            // get list of spaces at this venue
            List<Space> spaces = spaceDAO.GetAllSpaces(venueId);

            // Display all Space information: Name, open/close, rate, max occup
            foreach (Space space in spaces)
            {
                Console.WriteLine(space.ToString());
            }

            Console.WriteLine("What would you like to do next?");
            Console.WriteLine("1) Reserve a Space");
            Console.WriteLine("R) return to previous screen");
            string choice = Console.ReadLine();

            switch (choice.ToLower())
            {
                // prompt user to reserve space
                case "1":
                    ReserveSpace(venueId);
                    break;

                // or return to previous screen
                case "r":
                    return;
            }
        }

        public void ReserveSpace(int venueId)
        {
            // prompt for: start date
            Console.WriteLine("When do you need the space?");
            DateTime startDate = Convert.ToDateTime(Console.ReadLine());

            // prompt for: duration
            Console.WriteLine("How many days will you need the space?");
            int numberOfDays = Convert.ToInt32(Console.ReadLine());

            // prompt for: attendees
            Console.WriteLine("How many people will be in attendance?");
            int attendees = Convert.ToInt32(Console.ReadLine());

            // Display all spaces that meet search criterea, if any
            List<Space> spaces = spaceDAO.SearchTop5SpaceAvailability(venueId, startDate, numberOfDays);

            foreach (Space space in spaces)
            {
                space.DaysReserved = numberOfDays;
                DisplaySpaceForReservation(space);
            }

            // prompt user to select available Space
            Console.WriteLine("Which space would you like to reserve (enter 0 to cancel)?");
            int spaceId = Convert.ToInt32(Console.ReadLine());

            if (spaceId == 0)
            {
                return;
            }

            // select space indicated by user
            Space selectedSpace = (Space)(from space in spaces
                                where space.Id == spaceId
                                select space);

            // prompt user for a name to put on the reservation
            Console.WriteLine("Who is this reservation for?");
            string customerName = Console.ReadLine();

            // makes reservation and populate with the privided information
            Reservation newReservation = new Reservation();
            
            newReservation.StartDate = startDate;
            newReservation.EndDate = startDate.AddDays(numberOfDays);
            newReservation.NumberOfAttendes = attendees;
            newReservation.ReservedFor = customerName;
            newReservation.SpaceID = selectedSpace.Id;
            newReservation.TotalCost = selectedSpace.EstimatedCost;

            // add the reservation to the database and get back the resevation ID
            int newReservationID = reservationDAO.MakeReservation(newReservation);

            // Displays reservation info back to the user
            DisplayReservation(newReservation);
        }

        public void DisplayVenue(Venue venue)
        {
            Console.WriteLine(venue.Name);
            Console.WriteLine("Location: " + venue.City + ", " + venue.State);
            //Console.WriteLine("Categories" + String.Join(", ", venue.Categories));
            Console.WriteLine();
            Console.WriteLine(venue.Description);
            Console.WriteLine();

        }

        public void DisplaySpaceForReservation(Space space)
        {
            Console.WriteLine($"{space.Id} {space.Name} {space.DailyRate} {space.MaxOccupancy} {space.IsAccessible} {space.EstimatedCost}");
        }

        public void DisplayReservation(Reservation reservation)
        {
            Console.WriteLine($"Confirmation #: {reservation.ReservationID.GetHashCode()}");
            //Console.WriteLine($"Venue: {reservation}");
            Console.WriteLine($"Space: {reservation.SpaceID}");
            Console.WriteLine($"Reserved For: {reservation.ReservedFor}");
            Console.WriteLine($"Attendees: {reservation.NumberOfAttendes}");
            Console.WriteLine($"Arrival Date: {reservation.StartDate}");
            Console.WriteLine($"Depart Date: {reservation.EndDate}");
            Console.WriteLine($"Total Cost: {reservation.TotalCost}");
        }
    }
}
