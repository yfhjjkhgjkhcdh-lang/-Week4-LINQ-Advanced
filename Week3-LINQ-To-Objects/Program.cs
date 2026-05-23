using Week3_LINQ_To_Objects.Date;
using Week3_LINQ_To_Objects.models;

namespace Week3_LINQ_To_Objects
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Repository.LoadAllData();

            MethodSyntaxQuery();

            QuerySyntaxQuery();

            ProjectionQuery();

            SelectManyQuery();

            SortHousesQuery();

            SortHeatersQuery();

            TakeQuery();

            SkipQuery();

            TakeWhileQuery();

            AnyQuery();

            AllQuery();

            ContainsQuery();

            GroupByQuery();

            ToLookupQuery();

            JoinQuery();

            GroupJoinQuery();

            RangeQuery();

            SelectWithIndexQuery();

            DeferredExecutionQuery();

            ImmediateExecutionQuery();
        }

        static void PrintTitle(string title)
        {
            Console.WriteLine();
            Console.WriteLine($"========== {title} ==========");
        }

        static void MethodSyntaxQuery()
        {
            PrintTitle("Method Syntax");

            var heaters = Repository.Heaters;

            var powerfulHeaters =
                heaters.Where(h => h.PowerValue > 1500);

            foreach (var heater in powerfulHeaters)
            {
                Console.WriteLine(
                    $"Heater Id: {heater.HeaterId}, " +
                    $"Power: {heater.PowerValue}");
            }
        }

        static void QuerySyntaxQuery()
        {
            PrintTitle("Query Syntax");

            var powerfulHeatersQuery =
                from h in Repository.Heaters
                where h.PowerValue > 1500
                select h;

            foreach (var heater in powerfulHeatersQuery)
            {
                Console.WriteLine(
                    $"Heater Id: {heater.HeaterId}, " +
                    $"Power: {heater.PowerValue}");
            }
        }

        static void ProjectionQuery()
        {
            PrintTitle("Projection");

            var houses = Repository.Houses;

            var result = houses.Select(h => new
            {
                h.HouseId,

                OwnerName =
                    Repository.Owners
                    .FirstOrDefault(o => o.OwnerId == h.OwnerId)
                    ?.FullName,

                TotalMonthHours =
                    h.DailyUsages.Sum(d => d.HoursWorked)
            });

            foreach (var item in result)
            {
                Console.WriteLine(
                    $"House Id: {item.HouseId}, " +
                    $"Owner Name: {item.OwnerName}, " +
                    $"Total Hours: {item.TotalMonthHours}");
            }
        }

        static void SelectManyQuery()
        {
            PrintTitle("SelectMany");

            var usages = Repository.Houses

                .SelectMany(
                    h => h.Heaters,
                    (house, heater) => new
                    {
                        house,
                        heater
                    })

                .SelectMany(
                    x => Repository.DailyUsages
                    .Where(d => d.HeaterId == x.heater.HeaterId),

                    (x, usage) => new
                    {
                        x.house.Address,
                        x.heater.HeaterType,
                        usage.HoursWorked,
                        usage.UsageDate
                    });

            foreach (var item in usages.Take(10))
            {
                Console.WriteLine(
                    $"Address: {item.Address}, " +
                    $"Type: {item.HeaterType}, " +
                    $"Hours: {item.HoursWorked}");
            }
        }

        static void SortHousesQuery()
        {
            PrintTitle("Sort Houses");

            var houses = Repository.Houses;

            var sortedHouses = houses

                .Select(h => new
                {
                    h.HouseId,

                    TotalHours =
                        h.DailyUsages.Sum(d => d.HoursWorked)
                })

                .OrderByDescending(x => x.TotalHours);

            foreach (var item in sortedHouses)
            {
                Console.WriteLine(
                    $"House Id: {item.HouseId}, " +
                    $"Total Hours: {item.TotalHours}");
            }
        }

        static void SortHeatersQuery()
        {
            PrintTitle("Sort Heaters");

            var houses = Repository.Houses;

            var sortedHeaters = houses.Select(h => new
            {
                h.Address,

                SortedHeaters =
                    h.Heaters
                    .OrderByDescending(heater => heater.PowerValue)
            });

            foreach (var house in sortedHeaters)
            {
                Console.WriteLine($"Address: {house.Address}");

                foreach (var heater in house.SortedHeaters)
                {
                    Console.WriteLine(
                        $"Heater Power: {heater.PowerValue}");
                }
            }
        }

        static void TakeQuery()
        {
            PrintTitle("Take");

            var houses = Repository.Houses;

            var topHouses = houses.Take(5);

            foreach (var house in topHouses)
            {
                Console.WriteLine(house.Address);
            }
        }

        static void SkipQuery()
        {
            PrintTitle("Skip");

            var houses = Repository.Houses;

            var skipHouses = houses.Skip(2);

            foreach (var house in skipHouses)
            {
                Console.WriteLine(house.Address);
            }
        }

        static void TakeWhileQuery()
        {
            PrintTitle("TakeWhile");

            double runningSum = 0;

            var result = Repository.DailyUsages

                .OrderBy(d => d.UsageDate)

                .TakeWhile(d =>
                {
                    runningSum += d.HoursWorked;

                    return runningSum <= 100;
                });

            foreach (var item in result)
            {
                Console.WriteLine(
                    $"{item.UsageDate} - {item.HoursWorked}");
            }
        }

        static void AnyQuery()
        {
            PrintTitle("Any");

            var houses = Repository.Houses;

            var result = houses

                .SelectMany(h => h.Heaters)

                .Any(heater => heater.PowerValue > 2000);

            Console.WriteLine(
                $"Any Heater > 2000 : {result}");
        }

        static void AllQuery()
        {
            PrintTitle("All");

            var houses = Repository.Houses;

            var result = houses

                .SelectMany(h => h.DailyUsages)

                .All(d => d.HoursWorked <= 24);

            Console.WriteLine(
                $"All Hours <= 24 : {result}");
        }

        static void ContainsQuery()
        {
            PrintTitle("Contains");

            var hasSpecificPower =
                Repository.Heaters
                .Any(h => h.PowerValue == 1500);

            Console.WriteLine(
                $"Contains Power 1500 : {hasSpecificPower}");
        }

        static void GroupByQuery()
        {
            PrintTitle("GroupBy");

            var dailyUsages = Repository.DailyUsages;

            var result = dailyUsages

                .GroupBy(x => x.HouseId)

                .Select(g => new
                {
                    HouseId = g.Key,

                    AvgHours =
                        g.Average(d => d.HoursWorked)
                });

            foreach (var item in result)
            {
                Console.WriteLine(
                    $"House Id: {item.HouseId}, " +
                    $"Average Hours: {item.AvgHours}");
            }
        }

        static void ToLookupQuery()
        {
            PrintTitle("ToLookup");

            var heaters = Repository.Heaters;

            var lookup =
                heaters.ToLookup(h => h.HeaterType);

            var electricHeaters = lookup["Electric"];

            foreach (var heater in electricHeaters)
            {
                Console.WriteLine(
                    $"Heater Id: {heater.HeaterId}, " +
                    $"Power: {heater.PowerValue}");
            }
        }

        static void JoinQuery()
        {
            PrintTitle("Join");

            var houses = Repository.Houses;

            var owners = Repository.Owners;

            var result = houses.Join(
                owners,

                h => h.OwnerId,

                o => o.OwnerId,

                (house, owner) => new
                {
                    house.Address,
                    owner.FullName
                });

            foreach (var item in result)
            {
                Console.WriteLine(
                    $"Address: {item.Address}, " +
                    $"Owner: {item.FullName}");
            }
        }

        static void GroupJoinQuery()
        {
            PrintTitle("GroupJoin");

            var houses = Repository.Houses;

            var result = houses.GroupJoin(

                Repository.DailyUsages,

                h => h.HouseId,

                d => d.HouseId,

                (house, usages) => new
                {
                    house.Address,

                    TotalHours =
                        usages.Sum(u => u.HoursWorked)
                });

            foreach (var item in result)
            {
                Console.WriteLine(
                    $"Address: {item.Address}, " +
                    $"Total Hours: {item.TotalHours}");
            }
        }

        static void RangeQuery()
        {
            PrintTitle("Enumerable.Range");

            var last30Days = Enumerable.Range(0, 30)

                .Select(day =>
                    DateTime.Today.AddDays(-day));

            foreach (var date in last30Days)
            {
                Console.WriteLine(date);
            }
        }

        static void SelectWithIndexQuery()
        {
            PrintTitle("Select With Index");

            var last30Days = Enumerable.Range(0, 30)

                .Select(day =>
                    DateTime.Today.AddDays(-day));

            var indexedDays = last30Days

                .Select((date, index) => new
                {
                    DayIndex = index,
                    Date = date
                });

            foreach (var item in indexedDays)
            {
                Console.WriteLine(
                    $"Index: {item.DayIndex}, " +
                    $"Date: {item.Date}");
            }
        }

        static void DeferredExecutionQuery()
        {
            PrintTitle("Deferred Execution");

            var deferredQuery = Repository.Heaters
                .Where(h => h.PowerValue > 2000);

            Repository.Heaters.Add(new Heater
            {
                HeaterId = Guid.NewGuid(),

                HouseId = Repository.Houses.First().HouseId,

                HeaterType = "Electric",

                PowerValue = 3000
            });

            foreach (var heater in deferredQuery)
            {
                Console.WriteLine(heater.PowerValue);
            }
        }

        static void ImmediateExecutionQuery()
        {
            PrintTitle("Immediate Execution");

            var immediateQuery = Repository.Heaters

                .Where(h => h.PowerValue > 2000)

                .ToList();

            Repository.Heaters.Add(new Heater
            {
                HeaterId = Guid.NewGuid(),

                HouseId = Repository.Houses.First().HouseId,

                HeaterType = "Gas",

                PowerValue = 5000
            });

            foreach (var heater in immediateQuery)
            {
                Console.WriteLine(heater.PowerValue);
            }
        }
    }
}