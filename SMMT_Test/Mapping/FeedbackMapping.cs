using SMMT_Test.Dtos;
using SMMT_Test.Models;

namespace SMMT_Test.Mapping
{
    public class FeedbackMapping
    {
        public FeedbackDto MapToDto(Feedback feedback)
        {
            FeedbackDto feedbackDto = new FeedbackDto();
            PropertyCopier<Feedback, FeedbackDto>.Copy(feedback, feedbackDto);

            return feedbackDto;
        }

        public Feedback MapFromDto(Feedback feedback, FeedbackDto feedbackDto)
        {

            PropertyCopier<FeedbackDto, Feedback>.Copy(feedbackDto, feedback);

            return feedback;

        }
    }
}
