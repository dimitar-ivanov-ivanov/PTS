namespace Statistics
{
    public class StudentSummaryViewModel
    {
        public int StudentId { get; set; }

        public int Wikis { get; set; }

        public int Files { get; set; }

        public string Score { get; set; }

        public StudentSummaryViewModel(int studentId, int wikis, int files, string score)
        {
            this.StudentId = studentId;
            this.Wikis = wikis;
            this.Files = files;
            this.Score = score;
        }

        public override string ToString()
        {
            return "Потребител: " + StudentId + ", Редактирани Уикита: " + Wikis + ", Прочетени файлове: " + Files + ", Oценка: " + Score;
        }
    }
}
