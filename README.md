 Week 4 - LINQ Advanced
## 📌 Project Overview

IceCity is a heating management system designed to analyze house heating usage using LINQ to Objects in C#.

The system works with in-memory collections (`List<T>`) and performs analytical queries on:

* House Owners
* Houses
* Heaters
* Daily Usage Reports

The project focuses on:

* Functional Programming
* LINQ Query Operations
* Data Analysis
* Deferred vs Immediate Execution
* Collection Processing

---

# 🏗️ Project Structure

The project contains the following folders:

* **Models** → contains domain classes
* **Data** → contains Repository and sample data
* **docs** → contains LINQ explanations
* **Program.cs** → contains LINQ queries and execution methods

---

# 📚 LINQ Topics Covered

## 🔹 Functional Programming

Implemented:

* Method Syntax
* Query Syntax

Example:

```csharp
var heaters = Repository.Heaters
    .Where(h => h.PowerValue > 1500);
```

---

## 🔹 Projection

Used:

* Select
* SelectMany

Purpose:

* Transforming objects
* Flattening nested collections

---

## 🔹 Sorting

Implemented:

* OrderBy
* OrderByDescending

Example:

```csharp
.OrderByDescending(h => h.TotalHours)
```

---

## 🔹 Partitioning

Implemented:

* Take
* Skip
* TakeWhile

Purpose:

* Filtering partial data
* Pagination-like behavior

---

## 🔹 Quantifiers

Implemented:

* Any
* All
* Contains

Purpose:

* Checking conditions on collections

---

## 🔹 Grouping

Implemented:

* GroupBy
* ToLookup

Purpose:

* Aggregating usage statistics
* Grouping heaters by type

---

## 🔹 Join Operations

Implemented:

* Join
* GroupJoin

Purpose:

* Connecting houses with owners
* Connecting houses with usage reports

---

## 🔹 Generation Operations

Implemented:

* Enumerable.Range
* Select with Index

Purpose:

* Generating date ranges
* Adding index values to results

---

# ⚡ Pure vs Impure Functions

## ✅ Pure Function

A pure function:

* Always returns the same result for the same input
* Does not modify external data
* Has no side effects

Example:

```csharp
static double CalculateCost(double power, double hours)
{
    return power * hours;
}
```

---

## ❌ Impure Function

An impure function:

* Modifies shared data
* Changes external state
* Causes side effects

Example:

```csharp
static void AddUsage(List<DailyUsage> usages)
{
    usages.Add(new DailyUsage());
}
```

---

# ⏳ Deferred vs Immediate Execution

## Deferred Execution

The query executes only when enumerated.

Example:

```csharp
var query = Repository.Heaters
    .Where(h => h.PowerValue > 2000);
```

---

## Immediate Execution

The query executes immediately when using `ToList()`.

Example:

```csharp
var query = Repository.Heaters
    .Where(h => h.PowerValue > 2000)
    .ToList();
```

---
## 🔹 Element Operators

Implemented:

* First
* FirstOrDefault
* Single
* SingleOrDefault
* Last
* LastOrDefault
* ElementAt

Example:

```csharp
var firstHeater = heaters.First();

var heater =
    heaters.FirstOrDefault(
        h => h.PowerValue > 5000
    );
```

Purpose:

* Retrieving specific elements from collections
* Handling missing results safely

---

## 🔹 Equality Operators

Implemented:

* SequenceEqual

Example:

```csharp
var result =
    ids1.SequenceEqual(ids2);
```

Purpose:

* Comparing collections by values and order

---

## 🔹 Concatenation Operators

Implemented:

* Concat
* Append
* Prepend

Example:

```csharp
var allHeaters =
    owner1Heaters.Concat(owner2Heaters);

var appended =
    owner1Heaters.Append(newHeater);
```

Purpose:

* Combining sequences
* Adding elements without modifying source collections

---

## 🔹 Aggregation Operators

Implemented:

* Sum
* Min
* Max
* Average
* Aggregate

Example:

```csharp
var averagePower =
    heaters.Average(h => h.PowerValue);

var csv =
    houses.Select(h => h.OwnerId)
          .Aggregate((x, y) => $"{x},{y}");
```

Purpose:

* Producing summarized values from collections

---

## 🔹 Set Operators

Implemented:

* Distinct
* DistinctBy
* Union
* Intersect
* Except

Example:

```csharp
var union =
    set1.Union(set2);

var intersect =
    set1.Intersect(set2);
```

Purpose:

* Performing mathematical set operations on collections

---

## 🔹 Conversion Operators

Implemented:

* OfType
* Cast
* ToList
* ToArray
* ToDictionary
* ToLookup

Example:

```csharp
var heaterList =
    heaters.ToList();

var heaterDictionary =
    heaters.ToDictionary(
        h => h.HeaterId
    );
```

Purpose:

* Converting collections into different data structures

---

## 🔹 IEnumerable vs IQueryable

Covered:

* IEnumerable
* IQueryable
* Expression Trees

Example:

```csharp
IQueryable<Heater> queryable =
    heaters.AsQueryable();

queryable =
    queryable.Where(
        h => h.PowerValue > 1000
    );
```

Purpose:

* Understanding execution differences
* Understanding query translation

---

## 🔹 Custom LINQ Extensions

Implemented:

* Median Extension Method

Example:

```csharp
var medianPower =
    heaters
    .Select(h => (double)h.PowerValue)
    .Median();
```

Purpose:

* Creating reusable custom LINQ operators

---

## 🔹 LINQ Anatomy

Covered:

* Query Pipeline
* Deferred Execution
* Expression Trees

Example:

```csharp
var query =
    heaters
    .Where(h => h.PowerValue > 1000)
    .Select(h => h.HeaterId)
    .OrderBy(id => id);
```

Purpose:

* Understanding how LINQ executes internally


# 🛠️ Technologies Used

* C#
* .NET Console Application
* LINQ
* In-Memory Collections

---

# ✅ Conclusion

This project demonstrates practical usage of LINQ to Objects using real-world examples and different LINQ operations for data analysis and collection processing.
