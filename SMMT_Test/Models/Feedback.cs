namespace SMMT_Test.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string FeedbackMessage { get; set; } = string.Empty;
        public DateTime DateSubmitted { get; set; }

    }
}
