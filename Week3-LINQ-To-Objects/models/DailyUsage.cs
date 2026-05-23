using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3_LINQ_To_Objects.models
{
    public class DailyUsage
    {
        public Guid DailyUsageId { get; set; }

        public Guid HouseId { get; set; }

        public Guid HeaterId { get; set; }

        public DateTime UsageDate { get; set; }

        public double HoursWorked { get; set; }
        public double HeaterValue { get; set; }
    }

}
