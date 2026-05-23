using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3_LINQ_To_Objects.models
{
    public class House
    {
        public Guid HouseId { get; set; }

        public Guid OwnerId { get; set; }

        public string Address { get; set; }

        public string CityZone { get; set; }

        public List<Heater> Heaters { get; set; } = new();

        public List<DailyUsage> DailyUsages { get; set; } = new();
    }
}
