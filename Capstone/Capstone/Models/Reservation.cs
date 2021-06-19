using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int SpaceID { get; set; }
        public string SpaceName { get; set; }
        public string VenueName { get; set; }
        public int NumberOfAttendes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReservedFor { get; set; }
        public decimal TotalCost { get; set; }
    }

}
