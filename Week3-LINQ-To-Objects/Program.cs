using System.Linq.Expressions;
using Week3_LINQ_To_Objects.Date;
using Week3_LINQ_To_Objects.LinqExtensions;
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
            ElmentOperatorsQuery();
            QuantifiersQuery();
            EqualityOperators();
            ConcatOperators();
            AggregationOperators();
            SetOperators();
            IEnumerableVsIQueryable();
            Conversions();
            CustomExtension();
            LinqAnatomy();




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
        static void ElmentOperatorsQuery()
        {
            PrintTitle("Element Operators");
            Console.WriteLine("====first====");
            var heaters = Repository.Heaters;
            var firstHeater = heaters.First();
            try
            {
                var NOTFoundHeater = heaters.First(h => h.PowerValue > 10000);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("No heater found with power > 10000");
            }

            Console.WriteLine(
                $"First Heater Power: {firstHeater.PowerValue}");
            var FirstOrDefault = heaters
                .FirstOrDefault(h => h.PowerValue > 5000);
            if (FirstOrDefault == null)
            {
                Console.WriteLine("No heater found with power > 5000");
            }
            else
            {
                Console.WriteLine(
                    $"FirstOrDefault Heater Power: {FirstOrDefault.PowerValue}");
            }
            Console.WriteLine("====single====");

            var SingleHeater = heaters.Single(h => h.HeaterType == "Gas");
            try
            {
                var errorSingleHeater = heaters.Single(h => h.PowerValue > 1000);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Multiple heaters found with power > 1000");
            }
            var SingleOrDefaultHeater = heaters.SingleOrDefault(h => h.HeaterType == "Electric");
            if (SingleOrDefaultHeater == null)
            {
                Console.WriteLine("No heater found with type Electric");
            }
            else
            {
                Console.WriteLine(
                    $"SingleOrDefault Heater Power: {SingleOrDefaultHeater.PowerValue}");
            }
            Console.WriteLine("\n===== ElementAt =====");

            var element = heaters.ElementAt(2);

            Console.WriteLine($"ElementAt(2): {element.HeaterId}");

            try
            {
                heaters.ElementAt(10000);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"ElementAt Exception: {ex.Message}");
            }



            Console.WriteLine("\n===== Last =====");

            var last = heaters.Last();

            Console.WriteLine($"Last Heater: {last.HeaterId}");



            Console.WriteLine("\n===== LastOrDefault =====");

            var lastOrDefault =
                heaters.LastOrDefault(h => h.PowerValue > 999999);

            Console.WriteLine(lastOrDefault == null
                ? "No Heater Found"
                : lastOrDefault.HeaterId.ToString());
        }
        static void QuantifiersQuery()
        {
            PrintTitle("Quantifiers");
            var heaters = Repository.Heaters;
            var anyPowerful = heaters.Any(h => h.PowerValue > 2000);
            Console.WriteLine($"Any Heater > 2000: {anyPowerful}");
            var allSafe = heaters.All(h => h.PowerValue <= 5000);
            Console.WriteLine($"All Heaters <= 5000: {allSafe}");
            var containsSpecific = heaters
                .Select(h => h.PowerValue)
                .Contains(1500);
            Console.WriteLine($"Contains Heater with Power 1500: {containsSpecific}");

        }
        static void EqualityOperators()
        {
            Console.WriteLine("========== Equality Operators ==========\n");

            var heaters = Repository.Heaters;
            var ids1 = heaters
                .Take(3)
                .Select(h => h.HeaterId)
                .ToList();


            var ids2 = heaters
                .Take(3)
                .Select(h => h.HeaterId)
                .ToList();

            Console.WriteLine("SequenceEqual (Same Order)");
            Console.WriteLine(ids1.SequenceEqual(ids2));
            Console.WriteLine();




            var ids3 = ids1
                .OrderByDescending(x => x)
                .ToList();

            Console.WriteLine("SequenceEqual (Different Order)");
            Console.WriteLine(ids1.SequenceEqual(ids3));
            Console.WriteLine();



            Console.WriteLine("SequenceEqual After OrderBy");
            Console.WriteLine(
                ids1.OrderBy(x => x)
                    .SequenceEqual(
                        ids3.OrderBy(x => x)
                    )
            );
            Console.WriteLine();



            Console.WriteLine("Except Result");

            var differences = ids1.Except(ids3);

            foreach (var id in differences)
            {
                Console.WriteLine(id);
            }

            Console.WriteLine();



            Console.WriteLine("Reference Equality Example");

            var heater1 = new Heater
            {
                HeaterId = Guid.NewGuid()
            };

            var heater2 = new Heater
            {
                HeaterId = heater1.HeaterId
            };

            Console.WriteLine(object.ReferenceEquals(heater1, heater2));

            Console.WriteLine();



            Console.WriteLine("Value Equality Example");

            int a = 10;
            int b = 10;

            Console.WriteLine(a == b);

            Console.WriteLine();
        }
        static void ConcatOperators()
        {
            var heaters = Repository.Heaters;
            var houses = Repository.Houses;
            Console.WriteLine("========== Concat / Append / Prepend ==========\n");

            var owner1Heaters = heaters
                .Where(h => h.HeaterId == Guid.Parse("1"))
                .ToList();

            var owner2Heaters = heaters
                .Where(h => h.HeaterId == Guid.Parse("2"))
                .ToList();



            Console.WriteLine("Owner 1 Heaters Count:");
            Console.WriteLine(owner1Heaters.Count);

            Console.WriteLine("Owner 2 Heaters Count:");
            Console.WriteLine(owner2Heaters.Count);

            Console.WriteLine();



            Console.WriteLine("Concat Result");

            var concatenated =
                owner1Heaters.Concat(owner2Heaters);

            Console.WriteLine($"Count: {concatenated.Count()}");

            foreach (var heater in concatenated)
            {
                Console.WriteLine(heater.HeaterId);
            }

            Console.WriteLine();



            Console.WriteLine("Append Result");

            var appended =
                owner1Heaters.Append(owner2Heaters.First());

            Console.WriteLine($"Count: {appended.Count()}");

            foreach (var heater in appended)
            {
                Console.WriteLine(heater.HeaterId);
            }

            Console.WriteLine();



            Console.WriteLine("Prepend Result");

            var prepended =
                owner1Heaters.Prepend(owner2Heaters.First());

            Console.WriteLine($"Count: {prepended.Count()}");

            foreach (var heater in prepended)
            {
                Console.WriteLine(heater.HeaterId);
            }

            Console.WriteLine();
        }


        static void AggregationOperators()
        {
            Console.WriteLine("========== Aggregation Operators ==========\n");
            var heaters = Repository.Heaters;
            var usages = Repository.DailyUsages;
            var houses = Repository.Houses;



            Console.WriteLine("Sum");

            var heaterId = heaters.First().HeaterId;

            var totalHours = usages
                .Where(u => u.HeaterId == heaterId)
                .Sum(u => u.HeaterValue);

            Console.WriteLine($"Total Hours: {totalHours}");



            Console.WriteLine("\nMin");

            var minPower =
                heaters.Min(h => h.PowerValue);

            Console.WriteLine(minPower);



            Console.WriteLine("\nMax");

            var maxPower =
                heaters.Max(h => h.PowerValue);

            Console.WriteLine(maxPower);



            Console.WriteLine("\nAverage");

            var averagePower =
                heaters.Average(h => h.PowerValue);

            Console.WriteLine(averagePower);



            Console.WriteLine("\nSafe Average");

            var emptyList = new List<int>();

            var safeAverage =
                emptyList.Any()
                ? emptyList.Average()
                : 0;

            Console.WriteLine(safeAverage);



            Console.WriteLine("\nAggregate CSV");



            var csv =
                houses
                .Take(5)
                .Select(h => h.OwnerId.ToString())
                .Aggregate((x, y) => $"{x},{y}");

            Console.WriteLine(csv);
        }
        static void SetOperators()
        {
            Console.WriteLine("========== Set Operators ==========\n");
            var heaters = Repository.Heaters;
            var houses = Repository.Houses;


            var zones =
                houses.Select(h => h.CityZone)
                      .Distinct();

            Console.WriteLine("Distinct Zones");

            foreach (var zone in zones)
            {
                Console.WriteLine(zone);
            }





            var set1 =
                heaters.Take(5)
                       .Select(h => h.HeaterId);

            var set2 =
                heaters.Skip(3)
                       .Take(5)
                       .Select(h => h.HeaterId);

            var union =
                set1.Union(set2);

            Console.WriteLine("\nUnion");

            foreach (var id in union)
            {
                Console.WriteLine(id);
            }





            var intersect =
                set1.Intersect(set2);

            Console.WriteLine("\nIntersect");

            foreach (var id in intersect)
            {
                Console.WriteLine(id);
            }




            var except =
                set1.Except(set2);

            Console.WriteLine("\nExcept");

            foreach (var id in except)
            {
                Console.WriteLine(id);
            }





            var distinctPower =
                heaters.DistinctBy(h => h.PowerValue);

            Console.WriteLine("\nDistinctBy Power");

            foreach (var heater in distinctPower)
            {
                Console.WriteLine(
                    $"{heater.HeaterId} - {heater.PowerValue}"
                );
            }
        }

        static void IEnumerableVsIQueryable()
        {
            Console.WriteLine("========== IEnumerable vs IQueryable ==========\n");
            var heaters = Repository.Heaters;



            Console.WriteLine("IEnumerable Example");

            IEnumerable<Heater> enumerable =
                heaters.Where(h => h.PowerValue > 1000);

            Console.WriteLine($"Type: {enumerable.GetType()}");

            Console.WriteLine("Results:");

            foreach (var heater in enumerable.Take(3))
            {
                Console.WriteLine(heater.HeaterId);
            }

            Console.WriteLine();



            Console.WriteLine("IQueryable Example");

            IQueryable<Heater> queryable =
                heaters.AsQueryable();

            queryable =
                queryable.Where(h => h.PowerValue > 1000);

            Console.WriteLine($"Type: {queryable.GetType()}");

            Console.WriteLine("Results:");

            foreach (var heater in queryable.Take(3))
            {
                Console.WriteLine(heater.HeaterId);
            }

            Console.WriteLine();



            Console.WriteLine("Expression Tree");

            Expression<Func<Heater, bool>> expression =
               h => h.PowerValue > 1000;

            Console.WriteLine(expression);

            Console.WriteLine();



            Console.WriteLine("Compiled Expression");

            Func<Heater, bool> compiled =
                expression.Compile();

            var filtered =
                heaters.Where(compiled);

            foreach (var heater in filtered.Take(3))
            {
                Console.WriteLine(heater.HeaterId);
            }

            Console.WriteLine();



            Console.WriteLine("Queryable Expression");

            Console.WriteLine(queryable.Expression);

            Console.WriteLine();
        }

        static void Conversions()
        {
            Console.WriteLine("========== Conversion Operators ==========\n");
            var heaters = Repository.Heaters;



            Console.WriteLine("OfType");

            object[] mixed =
            {
                10,
                "Ahmed",
                heaters.First(),
                20,
                heaters.Last()
            };


            var heaterOnly =
                mixed.OfType<Heater>();

            foreach (var heater in heaterOnly)
            {
                Console.WriteLine(heater.HeaterId);
            }

            Console.WriteLine();



            Console.WriteLine("Cast");
            try
            {
                var invalidCast =
                    mixed.Cast<Heater>();
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Cast Exception: {ex.Message}");
            }

            object[] heaterObjects =
                heaters.Cast<object>()
                       .ToArray();

            var casted =
                heaterObjects.Cast<Heater>();

            foreach (var heater in casted.Take(3))
            {
                Console.WriteLine(heater.HeaterId);
            }

            Console.WriteLine();



            Console.WriteLine("ToList");

            var heaterList =
                heaters.ToList();

            Console.WriteLine(heaterList.Count);

            Console.WriteLine();



            Console.WriteLine("ToArray");

            var heaterArray =
                heaters.ToArray();

            Console.WriteLine(heaterArray.Length);

            Console.WriteLine();



            Console.WriteLine("ToDictionary");

            var heaterDictionary =
                heaters.ToDictionary(
                    h => h.HeaterId
                );

            Console.WriteLine(
                heaterDictionary.First().Key
            );

            Console.WriteLine();



            Console.WriteLine("ToLookup");

            var lookup =
                heaters.ToLookup(
                    h => h.PowerValue);

            foreach (var heater in lookup.First())
            {
                Console.WriteLine(heater.HeaterId);
            }

        }
        static void CustomExtension()
        {
            Console.WriteLine("========== Custom Extension ==========\n");
            var heaters = Repository.Heaters;

            var medianPower =
                heaters
                .Select(h => (double)h.PowerValue)
                .Median();

            Console.WriteLine(
                $"Median Power = {medianPower}"
            );
        }
        static void LinqAnatomy()
        {
            Console.WriteLine("========== LINQ Anatomy ==========\n");
            var heaters = Repository.Heaters;




            Console.WriteLine("Query Pipeline");

            var query =
                heaters
                .Where(h => h.PowerValue > 1000)
                .Select(h => h.HeaterId)
                .OrderBy(id => id);



            Console.WriteLine("Deferred Execution Created");



            Console.WriteLine("\nEnumeration Started");

            foreach (var id in query.Take(5))
            {
                Console.WriteLine(id);
            }



            Console.WriteLine("\nIQueryable Example");

            var queryable =
                heaters
                .AsQueryable()
                .Where(h => h.PowerValue > 1000);



            Console.WriteLine("\nExpression Tree:");

            Console.WriteLine(queryable.Expression);



            Console.WriteLine("\nExecution:");

            foreach (var heater in queryable.Take(5))
            {
                Console.WriteLine(heater.HeaterId);
            }
        }


    }





}




