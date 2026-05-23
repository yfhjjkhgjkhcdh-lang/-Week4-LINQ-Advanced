# Week 3 LINQ To Objects

## Pure vs Impure Functions

### Pure Function

الـ Pure Function هى function بترجع نفس النتيجة لنفس الـ input كل مرة، ومبتعملش تعديل على أى بيانات خارجية أو shared state.

الـ Pure Functions مفضلة داخل LINQ Pipelines لأنها:
- سهلة التوقع
- سهلة الـ debugging
- سهلة الاختبار
- لا تسبب side effects

Example:

```csharp
static double CalculateCost(double power, double hours)
{
    return power * hours;
}
```

هذه function تعتبر Pure لأن:
- نفس الـ input يعطى نفس الـ output دائمًا.
- لا تعدل أى list أو object خارجها.
- لا تغير حالة البرنامج.

---

### Impure Function

الـ Impure Function يمكن أن تعدل بيانات مشتركة أو تغير حالة البرنامج.

Example:

```csharp
static void AddUsage(List<DailyUsage> usages)
{
    usages.Add(new DailyUsage
    {
        DailyUsageId = Guid.NewGuid(),
        HoursWorked = 5
    });
}
```

هذه function تعتبر Impure لأنها:
- تعدل الـ shared list.
- تسبب side effects.
- تغير حالة البرنامج بعد تنفيذها.

---

## Deferred Execution

الـ Deferred Execution معناه أن الـ query لا يتم تنفيذها مباشرة.

يتم التنفيذ فقط عند استخدام:
- foreach
- ToList()
- Count()

Example:

```csharp
var deferredQuery = Repository.Heaters
    .Where(h => h.PowerValue > 2000);

Repository.Heaters.Add(new Heater
{
    HeaterId = Guid.NewGuid(),
    PowerValue = 3000
});

foreach (var heater in deferredQuery)
{
    Console.WriteLine(heater.PowerValue);
}
```

فى المثال:
- تم إنشاء الـ query أولًا.
- ثم تمت إضافة Heater جديد.
- وعند التنفيذ ظهر الـ heater الجديد لأن التنفيذ كان Deferred.

---

## Immediate Execution

الـ Immediate Execution يحدث عند استخدام:
- ToList()
- ToArray()
- Count()

Example:

```csharp
var immediateQuery = Repository.Heaters
    .Where(h => h.PowerValue > 2000)
    .ToList();

Repository.Heaters.Add(new Heater
{
    HeaterId = Guid.NewGuid(),
    PowerValue = 5000
});

foreach (var heater in immediateQuery)
{
    Console.WriteLine(heater.PowerValue);
}
```

فى المثال:
- الـ query تنفذت مباشرة بسبب ToList().
- إضافة بيانات جديدة بعد ذلك لم تؤثر على النتائج.
- لأن البيانات تم تخزينها بالفعل فى الذاكرة.