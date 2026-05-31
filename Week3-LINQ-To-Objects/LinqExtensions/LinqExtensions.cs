using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3_LINQ_To_Objects.LinqExtensions
{
    public static class LinqExtensions
    {
        public static double Median(
            this IEnumerable<double> source)
        {
            var numbers =
                source.OrderBy(x => x)
                      .ToList();

            if (!numbers.Any())
                return 0;

            int count =
                numbers.Count;

            if (count % 2 == 1)
            {
                return numbers[count / 2];
            }

            return
                (numbers[count / 2 - 1]
                 +
                 numbers[count / 2])
                 / 2.0;
        }
    }
}
