using ExelReader;

namespace Statistics
{
    public class StatisticsViewModel
    {
        private static ExelReader.ExelReader exelReader = new ExelReader.ExelReader();

        public const int NOT_INITIALIZED = Int32.MinValue;

        public static String PathToLogs { get; set; }

        public static String PathToResults { get; set; }

        public static String PathToStudentResults { get; set; }

        public Dictionary<int, int> AbsoluteFrequencies { get; set; }

        public Dictionary<int, String> RelativeFrequencies { get; set; }

        public List<double> Mode { get; set; }

        public List<StudentSummaryViewModel> StudentsSummary { get; set; }
        
        public static Decimal Median { get; set; }

        public static Double Average { get; set; }

        public static double Dispersion { get; set; }

        public static decimal StandardDeviation { get; set; }

        public static long Swing { get; set; }

        public static double Correlation { get; set; }

        public static List<int> WikisCount { get; set; }

        public static List<int> FilesCount { get; set; }

        public StatisticsViewModel()
        {
            exelReader.ReadLogs(PathToLogs + "\\Logs_Course A_StudentsActivities.xlsx");
            exelReader.updatedWikisPerId = exelReader.GetUserIdCount(exelReader.updatedWikis);
            exelReader.uploadedFilesPerId = exelReader.GetUserIdCount(exelReader.uploadedFiles);
            exelReader.ReadScores(PathToStudentResults + "\\Course A_StudentsResults_Year 1.xlsx");
            exelReader.ReadScores(PathToStudentResults + "\\Course A_StudentsResults_Year 2.xlsx");

            WikisCount = exelReader.updatedWikisPerId.Select(wiki => wiki.Value).ToList();
            FilesCount = exelReader.uploadedFilesPerId.Select(file => file.Value).ToList();
          
            FrequencyCalculator calculator = new FrequencyCalculator();
            AbsoluteFrequencies = calculator.GetAbsoluteFrequencies(WikisCount)
                .OrderBy(x=>x.Key)
                .ToDictionary(x=>x.Key,x=> x.Value);
            
            RelativeFrequencies = calculator.GetRelativeFrequencies(WikisCount)
                .Select(x =>
                {
                    String percent = x.Value + "%";
                    KeyValuePair<int, string> result = new KeyValuePair<int, string>(x.Key, percent);
                    return result;
                })
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);
            
            Mode = StatisticsCalculator.GetMode(WikisCount);
            
            StudentsSummary = new List<StudentSummaryViewModel>(exelReader.uploadedFilesPerId.Select(file =>
            {
                int userId = file.Key;
                int wikiCount = 0;
                int fileCount = file.Value;
                string score = "Без оценка";

                if (exelReader.updatedWikisPerId.ContainsKey(userId))
                {
                    wikiCount = exelReader.updatedWikisPerId[userId];
                }
                if (exelReader.scores.ContainsKey(userId))
                {
                    score = exelReader.scores[userId].ToString();
                }

                return new StudentSummaryViewModel(userId, wikiCount, fileCount, score);
            }).ToList());
        }

        public static void LoadMedian()
        {
            Median = StatisticsCalculator.GetMedian(WikisCount);
        }

        public static void LoadAverage()
        {
            Average = StatisticsCalculator.GetAverage(WikisCount);
        }

        public static void LoadDispersion()
        {
            Dispersion = DistractionCalculator.GetDispersion(WikisCount);
        }

        public static void LoadStandardDeviation()
        {
            StandardDeviation = DistractionCalculator.GetStandardDeviation(WikisCount);
        }

        public static void LoadSwing()
        {
            Swing = DistractionCalculator.GetSwing(WikisCount);
        }

        public static void LoadCorrelation()
        {
            exelReader.FillMissingUserIds();
            WikisCount = exelReader.updatedWikisPerId.Select(wiki => wiki.Value).ToList();
            FilesCount = exelReader.uploadedFilesPerId.Select(file => file.Value).ToList();

            Correlation = CorrelationCalculator.ComputeCoeff(WikisCount, FilesCount);

            exelReader.updatedWikisPerId = exelReader.GetUserIdCount(exelReader.updatedWikis);
            exelReader.uploadedFilesPerId = exelReader.GetUserIdCount(exelReader.uploadedFiles);
        }

        public static void ClearAll()
        {
            Median = NOT_INITIALIZED;
            Average = NOT_INITIALIZED;
            Dispersion = NOT_INITIALIZED;
            StandardDeviation = NOT_INITIALIZED;
            Swing = NOT_INITIALIZED;
            Correlation = NOT_INITIALIZED;
        }
    }
}
