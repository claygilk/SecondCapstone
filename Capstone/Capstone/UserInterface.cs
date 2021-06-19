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
                Console.WriteLine("----- Main Menu -----");
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
            Console.WriteLine("Which venue would you like to view?");

            List<Venue> venues = this.venueDAO.GetAllVenues();

            foreach (Venue v in venues)
            {
                Console.WriteLine(v.Id.ToString().PadLeft(2) + ")  " + v.Name);
            }
            Console.WriteLine();

            string choice = Console.ReadLine();

            if (choice.ToLower() == "r")
            {
                return;
            }

            else
            {
                bool choiceIsInt = int.TryParse(choice, out int venueNumber);

                while (venueNumber > venues.Count || venueNumber < 1 || !choiceIsInt)
                {
                    Console.WriteLine("Please select a valid venue number.");
                    choiceIsInt = int.TryParse(Console.ReadLine(), out venueNumber);
                }
                VenueDetails(venueNumber);
            }
        }

        public void VenueDetails(int venueId)
        {
            string choice = "";

            do
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

                choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    // 1. List all spaces at the current venue
                    case "1":
                        ListSpaces(venueId);
                        break;

                    // BONUS: 2. Search for Reservation - shows all spaces availble during a given chunk of time

                    // r. return to prev screen
                    case "r":
                        return;
                    default:
                        Console.WriteLine("Invalid Selection");
                        break;
                }

            } while (choice != "1" || choice != "r");


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
            Console.WriteLine("Name".PadRight(25) + "Open".PadRight(10) + "Close".PadRight(10) + "Rate".PadRight(15) + "Max Occupancy");
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
            // Create temporary reservation object to store user input before making reservation
            Reservation tempReservation = new Reservation();

            // prompt for: start date

            // While loop is used to continue to prompt user for a start date until they enter a valid date
            bool validDate = false;

            while (!validDate)
            {
                Console.WriteLine("When do you need the space?");

                DateTime tempDate;
                validDate = DateTime.TryParse(Console.ReadLine(), out tempDate);

                tempReservation.StartDate = tempDate;

                if (!validDate)
                {
                    Console.WriteLine("Please enter a valid date (mm/dd/yyyy).");
                }
            }

            // prompt for: duration
            int numberOfDays = CLIHelper.GetInteger("How many days will you need the space?");

            //bool validNumber = false;

            //while (!validNumber)
            //{
            //    int userNumber;
            //    Console.WriteLine("How many days will you need the space?");

            //    validNumber = int.TryParse(Console.ReadLine(), out userNumber);
            //    //numberOfDays = Convert.ToInt32(Console.ReadLine());
            //    tempReservation.ReservedFor = userNumber.ToString();
            //    Console.WriteLine("Please enter a whole number.");

            //}


            // prompt for: attendees
            int attendees = CLIHelper.GetInteger("How many people will be in attendance?");

            // Display all spaces that meet search criterea, if any
            List<Space> spaces = spaceDAO.SearchTop5SpaceAvailability(venueId, tempReservation.StartDate, numberOfDays);
            
            if (spaces.Count == 0)
            {
                Console.WriteLine("No Spaces Available");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("----- Available Spaces -----");
                foreach (Space space in spaces)
                {
                    space.DaysReserved = numberOfDays;
                    DisplaySpaceForReservation(space);
                }
            }

            // prompt user to select available Space
            Console.WriteLine();
            Console.WriteLine("Which space would you like to reserve (enter 0 to cancel)?");
            int spaceId = Convert.ToInt32(Console.ReadLine());
            
            
            if (spaceId == 0)
            {
                return;
            }

            // select space indicated by user
            var newspace = (from space in spaces
                            where space.Id == spaceId
                            select space);

            Space selectedSpace = newspace.First();

            // prompt user for a name to put on the reservation
            Console.WriteLine("Who is this reservation for?");
            string customerName = Console.ReadLine();

            // makes reservation and populate with the privided information
            tempReservation.EndDate = tempReservation.StartDate.AddDays(numberOfDays);
            tempReservation.NumberOfAttendes = attendees;
            tempReservation.ReservedFor = customerName;
            tempReservation.SpaceID = selectedSpace.Id;
            tempReservation.TotalCost = selectedSpace.EstimatedCost;
            tempReservation.SpaceName = selectedSpace.Name;
            tempReservation.VenueName = venueDAO.SelectVenues(venueId).Name;

            // add the reservation to the database and get back the resevation ID
            int newReservationID = reservationDAO.MakeReservation(tempReservation);

            // Displays reservation info back to the user
            Console.WriteLine();
            DisplayReservation(tempReservation);
            Console.WriteLine();
            Console.WriteLine("Press Enter to Return to the Previous Screen");
            Console.ReadLine();
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
            Console.WriteLine($"{space.Id} {space.Name} {space.DailyRate.ToString("c")} {space.MaxOccupancy} {space.IsAccessible} {space.EstimatedCost.ToString("c")}");
        }

        public void DisplayReservation(Reservation reservation)
        {
            Console.WriteLine($"Confirmation #: {reservation.ReservationID}");
            Console.WriteLine($"Venue: {reservation.VenueName}");
            Console.WriteLine($"Space: {reservation.SpaceName}");
            Console.WriteLine($"Reserved For: {reservation.ReservedFor}");
            Console.WriteLine($"Attendees: {reservation.NumberOfAttendes}");
            Console.WriteLine($"Arrival Date: {reservation.StartDate.Month}/{reservation.StartDate.Day}/{reservation.StartDate.Year}");
            Console.WriteLine($"Depart Date: {reservation.EndDate.Month}/{reservation.EndDate.Day}/{reservation.EndDate.Year}");
            Console.WriteLine($"Total Cost: {reservation.TotalCost:c}");
        }
    }
}
