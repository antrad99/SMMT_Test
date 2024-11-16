namespace SMMT_Test.Dtos
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string FeedbackMessage { get; set; } = string.Empty;
        public DateTime DateSubmitted { get; set; }
    }
}
