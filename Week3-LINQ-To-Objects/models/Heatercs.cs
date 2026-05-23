using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3_LINQ_To_Objects.models
{
    public class Heater
    {
        public Guid HeaterId { get; set; }

        public Guid HouseId { get; set; }

        public string HeaterType { get; set; }

        public double PowerValue { get; set; }
    }
}
