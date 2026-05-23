using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week3_LINQ_To_Objects.models;

namespace Week3_LINQ_To_Objects.Date
{
    public static class Repository
    {
        public static List<Owner> Owners { get; } = new();

        public static List<House> Houses { get; } = new();

        public static List<Heater> Heaters { get; } = new();

        public static List<DailyUsage> DailyUsages { get; } = new();

        private static readonly Random _rand = new Random(42);

        public static void LoadAllData()
        {
            LoadOwners();

            LoadHouses();

            LoadHeaters();

            EnsureMinimumHeaters(6);

            LoadDailyUsages();

            WireNavigation();
        }

        public static void LoadOwners()
        {
            Owners.Clear();

            Owners.AddRange(new[]
            {
            new Owner
            {
                OwnerId = Guid.NewGuid(),
                FullName = "Ahmed Hany",
                Email = "ahmed@example.com",
                Phone = "+201000000001"
            },

            new Owner
            {
                OwnerId = Guid.NewGuid(),
                FullName = "Mona Ali",
                Email = "mona@example.com",
                Phone = "+201000000002"
            },

            new Owner
            {
                OwnerId = Guid.NewGuid(),
                FullName = "Khaled Omar",
                Email = "khaled@example.com",
                Phone = "+201000000003"
            }
        });
        }

        public static void LoadHouses()
        {
            Houses.Clear();

            foreach (var owner in Owners)
            {
                for (int i = 1; i <= 2; i++)
                {
                    Houses.Add(new House
                    {
                        HouseId = Guid.NewGuid(),
                        OwnerId = owner.OwnerId,
                        Address = $"Street {i} - {owner.FullName}",
                        CityZone = $"Zone-{_rand.Next(1, 4)}"
                    });
                }
            }
        }

        public static void LoadHeaters()
        {
            Heaters.Clear();

            var heaterTypes = new[] { "Electric", "Gas", "Oil" };

            foreach (var house in Houses)
            {
                int count = _rand.Next(1, 4);

                for (int i = 0; i < count; i++)
                {
                    double power = 800 + _rand.Next(200, 2600);

                    var heater = new Heater
                    {
                        HeaterId = Guid.NewGuid(),
                        HouseId = house.HouseId,
                        HeaterType = heaterTypes[_rand.Next(0, heaterTypes.Length)],
                        PowerValue = Math.Round(power, 2)
                    };

                    Heaters.Add(heater);
                }
            }
        }

        private static void EnsureMinimumHeaters(int minCount)
        {
            if (Heaters.Count >= minCount)
                return;

            var heaterTypes = new[] { "Electric", "Gas", "Oil" };

            var housesArray = Houses.ToArray();

            int attempt = 0;

            while (Heaters.Count < minCount && attempt < 1000)
            {
                attempt++;

                var targetHouse = housesArray[_rand.Next(housesArray.Length)];

                double power = 800 + _rand.Next(200, 2600);

                Heaters.Add(new Heater
                {
                    HeaterId = Guid.NewGuid(),
                    HouseId = targetHouse.HouseId,
                    HeaterType = heaterTypes[_rand.Next(0, heaterTypes.Length)],
                    PowerValue = Math.Round(power, 2)
                });
            }
        }

        public static void LoadDailyUsages()
        {
            DailyUsages.Clear();

            var today = DateTime.Today;

            foreach (var heater in Heaters)
            {
                for (int dayOffset = 1; dayOffset <= 30; dayOffset++)
                {
                    var date = today.AddDays(-dayOffset);

                    double hours =
                        Math.Round(_rand.NextDouble() * 9.5 + 0.5, 2);

                    DailyUsages.Add(new DailyUsage
                    {
                        DailyUsageId = Guid.NewGuid(),
                        HouseId = heater.HouseId,
                        HeaterId = heater.HeaterId,
                        UsageDate = date,
                        HoursWorked = hours,
                        HeaterValue = heater.PowerValue
                    });
                }
            }
        }

        public static void WireNavigation()
        {
            foreach (var h in Houses)
            {
                h.Heaters.Clear();

                h.DailyUsages.Clear();
            }

            foreach (var heater in Heaters)
            {
                var house =
                    Houses.FirstOrDefault(x => x.HouseId == heater.HouseId);

                if (house != null)
                {
                    house.Heaters.Add(heater);
                }
            }

            foreach (var usage in DailyUsages)
            {
                var house =
                    Houses.FirstOrDefault(x => x.HouseId == usage.HouseId);

                if (house != null)
                {
                    house.DailyUsages.Add(usage);
                }
            }
        }
    }
}
