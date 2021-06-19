using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public string Name { get; set; }
        public bool IsAccessible { get; set; }
        public DateTime? OpenFrom { get; set; }
        public DateTime? OpenTo { get; set; }
        public decimal DailyRate { get; set; }
        public int MaxOccupancy { get; set; }
        public int DaysReserved { get; set; }
        public decimal EstimatedCost
        {
            get
            {
                return this.DaysReserved * this.DailyRate;
            }
        }

        public override string ToString()
        {
            if (this.OpenFrom.HasValue && this.OpenTo.HasValue)
            {
                return this.Name.PadRight(25) + this.OpenFrom.Value.Month.ToString().PadRight(10) + this.OpenTo.Value.Month.ToString().PadRight(10) + this.DailyRate.ToString("c").PadRight(15) +  this.MaxOccupancy;
            }
            return this.Name.PadRight(25)+ "NA".PadRight(10) + "NA".PadRight(10) + this.DailyRate.ToString("c").PadRight(15) +  this.MaxOccupancy;
        }
    }
}
