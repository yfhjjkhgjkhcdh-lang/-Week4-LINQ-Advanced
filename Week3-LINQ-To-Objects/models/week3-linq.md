
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


## Introduction

في هذا الأسبوع تم استكشاف مفاهيم LINQ المتقدمة مثل Element Operators و Equality Operators و Aggregation Operators و Set Operators و Conversion Operators و Expression Trees و Extension Methods بالإضافة إلى فهم كيفية عمل LINQ داخليًا.

---

# 1. Element Operators

تستخدم للحصول على عنصر واحد من التسلسل.

## First()

يقوم بإرجاع أول عنصر يطابق الشرط.

* يرمي InvalidOperationException إذا لم يجد أي عنصر.

## FirstOrDefault()

يقوم بإرجاع أول عنصر يطابق الشرط.

* يرجع القيمة الافتراضية إذا لم يجد أي عنصر.

## Single()

يقوم بإرجاع عنصرًا واحدًا فقط.

* يرمي Exception إذا لم يجد عنصرًا.
* يرمي Exception إذا وجد أكثر من عنصر.

## SingleOrDefault()

يشبه Single ولكن:

* يرجع القيمة الافتراضية إذا لم يجد عنصرًا.
* يرمي Exception إذا وجد أكثر من عنصر.

## ElementAt()

يقوم بإرجاع العنصر الموجود في Index معين.

## Last()

يقوم بإرجاع آخر عنصر.

## LastOrDefault()

يقوم بإرجاع آخر عنصر أو القيمة الافتراضية إذا كانت المجموعة فارغة.

### Conclusion

تستخدم Element Operators للحصول على عنصر واحد مع التعامل الآمن مع الحالات الفارغة باستخدام OrDefault.

---

# 2. Equality Operators

تستخدم لمقارنة المجموعات.

## SequenceEqual()

يقارن بين مجموعتين عنصرًا بعنصر.

الخصائص:

* حساس للترتيب.
* حساس لعدد العناصر.

مثال:

```csharp
list1.SequenceEqual(list2);
```

## Except()

يقوم بإرجاع العناصر الموجودة في المجموعة الأولى وغير الموجودة في الثانية.

مثال:

```csharp
list1.Except(list2);
```

## Equality Semantics

### Value Equality

تعتمد المقارنة على القيمة نفسها.

### Reference Equality

تعتمد المقارنة على مرجع الكائن في الذاكرة.

### Conclusion

SequenceEqual تستخدم للتحقق من تطابق المجموعات بينما Except تستخدم لمعرفة الاختلافات.

---

# 3. Concat / Append / Prepend

## Concat()

يقوم بدمج مجموعتين في Sequence واحدة.

## Append()

يقوم بإضافة عنصر واحد في نهاية التسلسل.

## Prepend()

يقوم بإضافة عنصر واحد في بداية التسلسل.

### Conclusion

تستخدم هذه العمليات لتجميع أو توسيع البيانات داخل التسلسلات.

---

# 4. Aggregation Operators

تستخدم لتحويل مجموعة عناصر إلى قيمة واحدة.

## Sum()

حساب مجموع القيم.

## Min()

إيجاد أصغر قيمة.

## Max()

إيجاد أكبر قيمة.

## Average()

حساب المتوسط الحسابي.

## Aggregate()

تنفيذ عملية تجميع مخصصة.

مثال:

```csharp
names.Aggregate((a,b) => $"{a},{b}");
```

الناتج:

```text
Ahmed,Ali,Omar
```

### Safe Average

يفضل استخدام Any() قبل Average() عند التعامل مع مجموعة قد تكون فارغة.

### Conclusion

Aggregation Operators تستخدم بكثرة في التقارير والإحصائيات وتحليل البيانات.

---

# 5. Set Operators

## Distinct()

إزالة العناصر المكررة.

## Union()

دمج مجموعتين مع إزالة التكرار.

## Intersect()

إرجاع العناصر المشتركة.

## Except()

إرجاع العناصر الموجودة في المجموعة الأولى فقط.

## DistinctBy()

إزالة التكرار اعتمادًا على خاصية معينة.

مثال:

```csharp
heaters.DistinctBy(h => h.Power);
```

### Conclusion

تساعد Set Operators على مقارنة المجموعات وإزالة التكرارات بسهولة.

---

# 6. IEnumerable vs IQueryable

## IEnumerable<T>

* يعمل داخل الذاكرة (In-Memory).
* يستخدم LINQ to Objects.
* يعتمد على Func.

مثال:

```csharp
heaters.Where(h => h.Power > 1000);
```

## IQueryable<T>

* يبني Expression Trees.
* يمكن ترجمته إلى SQL.
* يستخدم Query Providers.

مثال:

```csharp
heaters.AsQueryable();
```

## Comparison

| IEnumerable     | IQueryable      |
| --------------- | --------------- |
| In-Memory       | Translatable    |
| Uses Func       | Uses Expression |
| LINQ to Objects | Query Provider  |

### Conclusion

IQueryable يسمح ببناء الاستعلام وترجمته قبل التنفيذ مما يحسن الأداء مع قواعد البيانات.

---

# 7. Conversion Operators

## OfType()

تصفية العناصر حسب النوع.

## Cast()

تحويل العناصر إلى نوع محدد.

## ToList()

تحويل النتائج إلى List.

## ToArray()

تحويل النتائج إلى Array.

## ToDictionary()

تحويل النتائج إلى Dictionary.

## ToLookup()

تحويل النتائج إلى Lookup.

### Dictionary vs Lookup

Dictionary:

```text
Key → Value
```

Lookup:

```text
Key → Multiple Values
```

### Conclusion

توفر Conversion Operators طرقًا متعددة لتحويل البيانات إلى الهياكل المناسبة.

---

# 8. Custom Extension Method

تم إنشاء Extension Method باسم:

```csharp
Median(this IEnumerable<double> source)
```

لحساب الوسيط (Median).

### الحالات المدعومة

* Empty Collection
* Odd Count
* Even Count

### Example

```text
1 2 3 4 5
Median = 3
```

```text
1 2 3 4
Median = 2.5
```

### Conclusion

تساعد Extension Methods على إضافة وظائف جديدة قابلة لإعادة الاستخدام وتحسين قابلية قراءة الكود.

---

# 9. LINQ Anatomy

يوضح هذا الجزء كيفية عمل LINQ داخليًا.

## Query Pipeline

```text
Source
 ↓
Where
 ↓
Select
 ↓
OrderBy
 ↓
Deferred Execution
 ↓
Enumeration
```

## Deferred Execution

عند إنشاء Query لا يتم تنفيذها مباشرة.

مثال:

```csharp
var query = heaters.Where(h => h.Power > 1000);
```

يتم التنفيذ عند:

```csharp
ToList()
ToArray()
Count()
foreach
```

## Expression Trees

تمثل الكود على شكل شجرة بيانات يمكن فحصها أو ترجمتها.

مثال:

```csharp
Expression<Func<Heater,bool>>
```

## Query Providers

تقوم Query Providers بتحويل Expression Trees إلى أوامر قابلة للتنفيذ مثل SQL.

### Conclusion

يعتمد LINQ على بناء Query Pipeline و Expression Trees قبل تنفيذ الاستعلام فعليًا.

---

# Final Notes

تم تنفيذ جميع المهام المطلوبة:

* Element Operators
* Equality Operators
* Concat / Append / Prepend
* Aggregation Operators
* Set Operators
* IEnumerable vs IQueryable
* Conversion Operators
* Custom Extension Method (Median)
* LINQ Anatomy

وقد تم عرض النتائج داخل Console Application مع توضيح المفاهيم المتقدمة الخاصة بـ LINQ.

