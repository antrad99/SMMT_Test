using Microsoft.EntityFrameworkCore;
using Serilog;
using SMMT_Test.Data;
using SMMT_Test.Dtos;
using SMMT_Test.Mapping;
using SMMT_Test.Models;

namespace SMMT_Test.Services
{
    public class FeedbackService: IFeedbackService
    {
        private readonly ApplicationDbContext _context;

        public FeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FeedbackDto>> GetAllFeedback()
        {
            var feedbacks = await _context.Feedback.OrderBy(x => x.DateSubmitted).ToListAsync();

            var feedbackDtos = new List<FeedbackDto>();
            var feedbackMappping = new FeedbackMapping();

            foreach (var feedback in feedbacks)
                feedbackDtos.Add(feedbackMappping.MapToDto(feedback));

            return feedbackDtos;
        }

        public async Task DeleteFeedback(int id)
        {
            var feedback = await _context.Feedback.FirstOrDefaultAsync(t => t.Id == id);

            if (feedback != null)
            {
                _context.Feedback.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<FeedbackDto> AddFeedback(FeedbackDto feedbackDto)
        {
            var feedbackMapping = new FeedbackMapping();
            var feedback = feedbackMapping.MapFromDto(new Feedback(), feedbackDto);
            feedback.DateSubmitted = DateTime.UtcNow;

            await _context.Feedback.AddAsync(feedback);

            await _context.SaveChangesAsync();

            return feedbackMapping.MapToDto(feedback);
        }

        public async Task UpdateFeedback(FeedbackDto feedbackDto)
        {
            var feedback = await _context.Feedback.FirstOrDefaultAsync(x => x.Id == feedbackDto.Id);
            if (feedback != null)
            {
                var feedbackMapping = new FeedbackMapping();

                feedback = feedbackMapping.MapFromDto(feedback, feedbackDto);
                feedback.DateSubmitted = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }
    }
}
