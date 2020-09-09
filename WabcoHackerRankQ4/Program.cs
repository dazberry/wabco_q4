using System;
using System.Collections.Generic;
using System.Linq;

namespace WabcoHackerRankQ4
{
    class Program
    {
        /*
            * Complete the 'selectStock' function below.
            *
            * The function is expected to return an INTEGER.
            * The function accepts following parameters:
            *  1. INTEGER saving
            *  2. INTEGER_ARRAY currentValue
            *  3. INTEGER_ARRAY futureValue
            */

        public static IEnumerable<T> SkipByIndex<T>(IEnumerable<T> values, int skipIndex)
        {
            foreach ((T value, int index) val in values.Select((x, i) => (x, i)))
            {
                if (val.index != skipIndex)
                    yield return val.value;
            }
        }

        public static int CalculateMaxReturn(int remainingSavings, int currentGain, (int value, int gain) currentValueAndGain, IEnumerable<(int value, int gain)> currentValuesAndGains)
        {
            if (remainingSavings - currentValueAndGain.value < 0)
                return currentGain;
            else
            {
                var valuesInRange = currentValuesAndGains
                        .Where(x => remainingSavings - x.value >= 0);

                if (valuesInRange.Any())
                    return valuesInRange.Select((x, i) =>
                        CalculateMaxReturn(
                            remainingSavings - currentValueAndGain.value,
                            currentGain + currentValueAndGain.gain,
                            x,
                            SkipByIndex(currentValuesAndGains, i)))
                        .Max();
            }
            return currentGain + currentValueAndGain.gain;
        }

        public static int CalculateMaxReturn(int saving, IEnumerable<(int value, int gain)> currentValuesAndGains) =>
            currentValuesAndGains.Select((x, i) =>
                CalculateMaxReturn(saving, 0, x, SkipByIndex(currentValuesAndGains, i)))
            .Max();

        public static int SelectStock(int saving, List<int> currentValue, List<int> futureValue)
        {
            var currentValuesAndGains =
                    currentValue.Zip(futureValue, (x, y) => (x, y - x))
                    .Where(((int cost, int gain) x) => saving >= x.cost && x.gain > 0);

            var result = CalculateMaxReturn(saving, currentValuesAndGains);
            return result;
        }

        public static void Main()
        {
            var currentAndFutureValues = new[]
            {
                (10, 10), (10, 4), (19, 15),
            };

            Console.WriteLine(SelectStock(20,
                currentAndFutureValues.Select(x => x.Item1).ToList(),
                currentAndFutureValues.Select(x => x.Item1 + x.Item2).ToList()));
        }

    }
}
