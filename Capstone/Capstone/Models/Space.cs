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
        public string DisplayAccessability
        {
            get
            {
                if (this.IsAccessible)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
        }
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
                return this.Name.PadRight(25) + MonthAbbreviation(this.OpenFrom.Value.Month).PadRight(10) + MonthAbbreviation(this.OpenTo.Value.Month).PadRight(10) + this.DailyRate.ToString("c").PadRight(15) + this.MaxOccupancy;
            }
            return this.Name.PadRight(25) + "NA".PadRight(10) + "NA".PadRight(10) + this.DailyRate.ToString("c").PadRight(15) + this.MaxOccupancy;
        }

        public string MonthAbbreviation(int month)
        {
            switch (month)
            {
                case 1:
                    return "Jan.";
                case 2:
                    return "Feb.";
                case 3:
                    return "Mar.";
                case 4:
                    return "Apr.";
                case 5:
                    return "May";
                case 6:
                    return "Jun.";
                case 7:
                    return "Jul.";
                case 8:
                    return "Aug.";
                case 9:
                    return "Sep.";
                case 10:
                    return "Oct.";
                case 11:
                    return "Nov.";
                case 12:
                    return "Dec.";
                default:
                    return "NA";
            }
        }
    }
}
