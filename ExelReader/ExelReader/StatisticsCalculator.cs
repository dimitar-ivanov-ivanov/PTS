using System.Collections.ObjectModel;

namespace ExelReader
{
    public class StatisticsCalculator
    {
        public static Decimal GetMedian(List<int> numbers)
        {
            int count = numbers.Count;
            decimal median = 0;

            if (count == 0)
            {
                return 0;
            }

            if (count % 2 == 0)
            {
                long a = numbers[count / 2 - 1];
                long b = numbers[count / 2];
                median = (a + b) / 2m;
            }
            else
            {
                median = numbers[count / 2];
            }
            return median;
        }

        public static double GetAverage(List<int> numbers)
        {
            if (numbers.Count == 0)
            {
                return 0;
            }

            double average = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                average += numbers[i];
            }
            average /= numbers.Count;
            return average;
        }

        public static double GetAverageOfDouble(List<double> numbers)
        {
            if (numbers.Count == 0)
            {
                return 0;
            }

            double average = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                average += numbers[i];
            }
            average /= numbers.Count;
            return average;
        }

        public static List<double> GetMode(List<int> numbers)
        {
            long mode = 0;
            long max = 0;
            List<double> result = new List<double>();
            if (numbers != null && numbers.Count() > 0)
            {
                Dictionary<long, long> counts = new Dictionary<long, long>();
                foreach (long element in numbers)
                {
                    if (counts.ContainsKey(element))
                        counts[element]++;
                    else
                        counts.Add(element, 1);
                }

                counts = counts.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                foreach (KeyValuePair<long, long> count in counts)
                {
                    mode = count.Key;
                    if (count.Value > max)
                    {
                        max = count.Value;
                        result = new List<double>();
                        result.Add(mode);
                    }
                    else if (count.Value == max)
                    {
                        //ако 2-те числа са съседни -> средно аритметично
                        result.Add(mode);
                    }
                }
            }

            if (max == 1 && result.Count == 1)
            {
                result = new List<double>();
            }

            bool modeIsAverage = result.Count > 0 && AreNeighbours(result, numbers);

            if (modeIsAverage)
            {
                double average = StatisticsCalculator.GetAverageOfDouble(result);
                result = new List<double>() { average };
            }

            return result;
        }

        private static bool AreNeighbours(List<double> result, List<int> numbers)
        {
            // what if we have 4 different numbers 
            // get the index of the first
            // loop until you reach a different number 
            // if that number is not a part of result -> numbers are not neighbours -> return (false)(return result as is) 
            // if that numer is part of result -> take it as the number current num and continue 
            // everytime you switch to new number -> count --
            // when count reaches == 0 -> we have exhausted our options -> all numbers are neighbours -> return average of all of them

            double currentNum = result[0];
            int index = GetFirstIndexOfNum(currentNum, numbers);
            int count = result.Count;
            for (int i = index; i < numbers.Count; i++)
            {
                if (count == 0)
                {
                    return true;
                }
                if (numbers[i] != currentNum)
                {
                    if (result.Contains(numbers[i]))
                    {
                        currentNum = numbers[i];
                        count--;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return count == 1;
        }

        private static int GetFirstIndexOfNum(double num, List<int> numbers)
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] == num)
                {
                    return i;
                }
            }

            throw new ArgumentException("Number is not part of numbers array. Incorrect Mode");
        }
    }
}
