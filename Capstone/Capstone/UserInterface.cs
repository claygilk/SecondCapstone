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
        /// <summary>
        /// The main menu which allows the user to navigate to the ViewVenue() menu or quit the program.
        /// </summary>
        public void MainMenu()
        {
            bool keepRunning = true;

            // Program continues to run until the users selects "Q" from the main menu
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

                    // Loop continues until the user makes a valid selection
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }

        /// <summary>
        /// Lists all venues in the database and then prompts the user to select one to view.
        /// Navigates to the VenueDetails() menu, once user selects a venue from the list.
        /// Returns to the main menu when complete.
        /// </summary>
        public void ViewVenue()
        {
            Console.WriteLine("Which venue would you like to view?");

            // Creates a list of all venues in the database returned by the VenueDAO object 
            List<Venue> venues = this.venueDAO.GetAllVenues();

            // Displays the name and Id of each venue in alphabetical order
            foreach (Venue v in venues)
            {
                Console.WriteLine(v.Id.ToString().PadLeft(2) + ")  " + v.Name);
            }
            Console.WriteLine();

            // gets input from user
            string choice = Console.ReadLine().ToLower();

            // If user selects "r", returns to the MainMenu()
            if (choice == "r")
            {
                return;
            }

            // For all other input, this else block ensures that the user eneters a valid venue id number
            else
            {
                // TryParse() creates an int variable and a bool variable
                // 'choiceIsIn' is true if the input is an integer
                bool choiceIsInt = int.TryParse(choice, out int venueNumber);

                // while loop checks that choice is an integer that corresponds to an exisiting venue number
                while (venueNumber > venues.Count || venueNumber < 1 || !choiceIsInt)
                {
                    Console.WriteLine("Please select a valid venue number.");
                    choiceIsInt = int.TryParse(Console.ReadLine(), out venueNumber);
                }

                // navigates to the VenueDetails() menu for the venue indicated by the user
                VenueDetails(venueNumber);
            }
        }

        /// <summary>
        /// Displays VenueDetails() then prompts the user to either navigate to the ListSpaces() menu,
        /// or return to the previous menu
        /// </summary>
        /// <param name="venueId"></param>
        public void VenueDetails(int venueId)
        {
            // this variable which stores the user's input is initialized outside of the do-while loop
            // so that it can be used in the while conidition and within the do-block
            string choice = "";

            do
            {
                // venueDAO object is used to query the database and return a C# object that 
                // corresponds to the venuse selected by the user
                Venue venue = venueDAO.SelectVenues(venueId);

                // This Method displays the following Venue information:
                // Name, Location (City, State), and Description
                DisplayVenue(venue);

                // Prompt user either view spaces at a venue
                // or return to the previous menu
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

                    // tells user their selection is invalid and then repeats the do-while loop
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

        /// <summary>
        /// This menu displays all the spaces at a given venue.
        /// The user can either navigate to the ReserveSpace() menu 
        /// or the previous menue
        /// </summary>
        /// <param name="venueId"></param>
        public void ListSpaces(int venueId)
        {
            // the variable that stores the user's input is initialized outside the following do-while loop
            // so that it can be used in the while-condition and the do-block
            string choice = "";

            // this loop repeats until the user makes a valid selection
            do
            {
                // get list of spaces at this venue by using a spaceDAO object to query the database
                List<Space> spaces = spaceDAO.GetAllSpaces(venueId);

                // Display all Space information: Name, open/close, rate, max occup
                Console.WriteLine("Name".PadRight(25) + "Open".PadRight(10) + "Close".PadRight(10) + "Rate".PadRight(15) + "Max Occupancy");
                foreach (Space space in spaces)
                {
                    Console.WriteLine(space.ToString());
                }

                // Prompt user to reserve a space 
                // or return to the previous menu
                Console.WriteLine("What would you like to do next?");
                Console.WriteLine("1) Reserve a Space");
                Console.WriteLine("R) return to previous screen");

                choice = Console.ReadLine().ToLower();
                switch (choice)
                {
                    // prompt user to reserve space
                    case "1":
                        ReserveSpace(venueId);
                        break;

                    // or return to previous screen
                    case "r":
                        return;

                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            } while (choice != "1" || choice != "r");
        }

        public void ReserveSpace(int venueId)
        {
            // Create temporary reservation object to store user input before making reservation
            Reservation tempReservation = new Reservation();

            GetDatesAndAttendees(tempReservation, out int numberOfDays);

            // A spaceDAO object is used to query the databse and return a list of (up to) 5 spaces that meet the user's search criteria
            // results are ordered by from cheapest to most expensive
            List<Space> spaces = spaceDAO.SearchTop5SpaceAvailability(venueId, tempReservation.StartDate, numberOfDays);
            spaces.RemoveAll(space => (space.MaxOccupancy < tempReservation.NumberOfAttendes));
            // If no spaces meet the user's criteria they are informed
            if (spaces.Count == 0)
            {
                Console.WriteLine("No Spaces Available");
            }
            // Otherwise, available spaces are listed
            else
            {
                Console.WriteLine();
                Console.WriteLine("----- Available Spaces -----");
                Console.WriteLine("Space#".PadRight(10) + "Name".PadRight(40) + "DailyRate".PadRight(15) + "MaxOccup.".PadRight(10) + "Accessible?".PadRight(15) + "TotalCost");
                foreach (Space space in spaces)
                {
                    // the number of days variable is assigned to the .DaysReserved proprtey
                    // so that the space object can calculate the total price
                    space.DaysReserved = numberOfDays;
                    
                    DisplaySpaceForReservation(space);
                }
            }

            Space selectedSpace = GetNameAndSpaceOfReservation(spaces, out string customerName);

            // makes reservation and populate with the privided information
            tempReservation.EndDate = tempReservation.StartDate.AddDays(numberOfDays);
            tempReservation.ReservedFor = customerName;
            tempReservation.SpaceID = selectedSpace.Id;
            tempReservation.TotalCost = selectedSpace.EstimatedCost;
            tempReservation.SpaceName = selectedSpace.Name;
            tempReservation.VenueName = venueDAO.SelectVenues(venueId).Name;

            // add the reservation to the database and get back the resevation ID
            int newReservationID = reservationDAO.MakeReservation(tempReservation);

            if (newReservationID != 0)
            {
                // Displays reservation info back to the user
                Console.WriteLine();
                DisplayReservation(tempReservation);
                Console.WriteLine();
                Console.WriteLine("Press Enter to Return to the Previous Screen");
                Console.ReadLine();
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spaces"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        public Space GetNameAndSpaceOfReservation(List<Space> spaces, out string customerName)
        {
            customerName = "";
            Space selectedSpace = new Space();

            // prompt user to select available Space
            Console.WriteLine();
            Console.WriteLine("Which space would you like to reserve (enter 0 to cancel)?");
            int spaceId = Convert.ToInt32(Console.ReadLine());


            if (spaceId == 0)
            {
                return selectedSpace;
            }

            // select space indicated by user
            var newspace = (from space in spaces
                            where space.Id == spaceId
                            select space);

            selectedSpace = newspace.First();

            // prompt user for a name to put on the reservation
            Console.WriteLine("Who is this reservation for?");
            customerName = Console.ReadLine();

            return selectedSpace;

        }

        private static void GetDatesAndAttendees(Reservation tempReservation, out int numberOfDays)
        {
            // This bool is initialized before the following while loop so that it can be used
            // both in the condition and the body of the following while loop
            bool validDate = false;

            // While loop is used to continue to prompt user for a start date until they enter a valid date
            while (!validDate)
            {
                // Prompt user for start date:
                Console.WriteLine("When do you need the space?");

                // declare date time variable to store user input, if it is in the correct format
                DateTime tempDate;
                validDate = DateTime.TryParse(Console.ReadLine(), out tempDate);

                // if the user input can be parsed as a date it will be stored in the ".StartDate" property
                tempReservation.StartDate = tempDate;

                if (!validDate)
                {
                    // informs user of expected date format
                    Console.WriteLine("Please enter a valid date (mm/dd/yyyy).");
                }
            }

            // Prompt user for duration
            numberOfDays = CLIHelper.GetInteger("How many days will you need the space?");

            // Prompt user for number of attendees
            tempReservation.NumberOfAttendes = CLIHelper.GetInteger("How many people will be in attendance?");
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
            Console.WriteLine(space.Id.ToString().PadRight(10) + space.Name.PadRight(40) + space.DailyRate.ToString("c").PadRight(15) + space.MaxOccupancy.ToString().PadRight(10) + space.DisplayAccessability.PadRight(15) + space.EstimatedCost.ToString("c"));
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
