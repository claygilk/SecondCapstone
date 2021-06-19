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
                return $"{this.Name} {this.OpenFrom.Value.Month}  {this.OpenTo.Value.Month}  {this.DailyRate.ToString("c")}  {this.MaxOccupancy}";
            }
            return $"{this.Name} NA  NA  {this.DailyRate.ToString("c")}  {this.MaxOccupancy}";
        }
    }
}
