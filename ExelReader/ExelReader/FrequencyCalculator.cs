namespace ExelReader
{
    public class FrequencyCalculator
    {
        public virtual Dictionary<int, int> GetAbsoluteFrequencies(List<int> wikis)
        {
            Dictionary<int, int> frequencies = new Dictionary<int, int>();
            wikis.ForEach(wiki => {
                if (!frequencies.ContainsKey(wiki))
                {
                    frequencies.Add(wiki, 0);
                }
                frequencies[wiki]++;
            });

            return frequencies;
        }

        public virtual Dictionary<int, decimal> GetRelativeFrequencies(List<int> wikis)
        {
            Dictionary<int, int> absoluteFrequencies = GetAbsoluteFrequencies(wikis);
            Dictionary<int, decimal> relativeFrequencies = new Dictionary<int, decimal>();
            foreach (var item in absoluteFrequencies)
            {
                relativeFrequencies.Add(item.Key, GetRelativeFrequency(item.Value, wikis.Count));
            }

            return relativeFrequencies;
        }

        private static decimal GetRelativeFrequency(long frequency, int numbersCount)
        {
            decimal relativeFrequency = (Decimal)(frequency * 100) / numbersCount;
            return Decimal.Round(relativeFrequency, 1, MidpointRounding.AwayFromZero);
        }
    }
}