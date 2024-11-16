using SMMT_Test.Data;
using SMMT_Test.Dtos;

namespace SMMT_Test.Services
{
    public interface IFeedbackService
    {
        Task<List<FeedbackDto>> GetAllFeedback();
        Task DeleteFeedback(int id);
        Task<FeedbackDto> AddFeedback(FeedbackDto feedbackDto);
        Task UpdateFeedback(FeedbackDto feedbackDto);
    }
}
