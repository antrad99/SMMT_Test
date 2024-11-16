using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SMMT_Test.Data;
using SMMT_Test.Dtos;
using SMMT_Test.Models;
using SMMT_Test.Services;

namespace Xunit_Tests
{
    public class FeedbackServiceTests
    {
        private ServiceCollection _services = new ServiceCollection();
        private Feedback feedback1;
        private Feedback feedback2;
        public FeedbackServiceTests()
        {
            _services.AddDbContext<ApplicationDbContext>(o =>
                        o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            feedback1 = new Feedback()
            {
                Id = 1,
                CustomerName = "Name1",
                FeedbackMessage = "Message1",
                DateSubmitted = new DateTime(2024, 11, 16, 11, 10, 14)
            };

            feedback2 = new Feedback()
            {
                Id = 2,
                CustomerName = "Name2",
                FeedbackMessage = "Message2",
                DateSubmitted = new DateTime(2024, 11, 17, 12, 23,24)
            };

        }

        [Fact]
        public async void GetAllFeedback()
        {
            using (var provider = _services.BuildServiceProvider())
            {
                using (var scope = provider.CreateScope())
                {

                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    context.Feedback.Add(feedback1);
                    context.Feedback.Add(feedback2);

                    context.SaveChanges();

                    var feedbackService = new FeedbackService(context);

                    var results = await feedbackService.GetAllFeedback();


                    Assert.Equal(2, results.Count);

                    Assert.Equal(feedback1.Id, results[0].Id);
                    Assert.Equal(feedback1.CustomerName, results[0].CustomerName);
                    Assert.Equal(feedback1.FeedbackMessage, results[0].FeedbackMessage);
                    Assert.Equal(feedback1.DateSubmitted, results[0].DateSubmitted);

                    Assert.Equal(feedback2.Id, results[1].Id);
                    Assert.Equal(feedback2.CustomerName, results[1].CustomerName);
                    Assert.Equal(feedback2.FeedbackMessage, results[1].FeedbackMessage);
                    Assert.Equal(feedback2.DateSubmitted, results[1].DateSubmitted);

                }
            }
        }

        [Fact]
        public async void DeleteFeedback()
        {
            using (var provider = _services.BuildServiceProvider())
            {
                using (var scope = provider.CreateScope())
                {

                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    context.Feedback.Add(feedback1);

                    context.SaveChanges();

                    var feedbackService = new FeedbackService(context);

                    await feedbackService.DeleteFeedback(1);


                    var results = await feedbackService.GetAllFeedback();

                    Assert.Empty(results);

                }
            }
        }

        [Fact]
        public async void AddFeedback()
        {
            using (var provider = _services.BuildServiceProvider())
            {
                using (var scope = provider.CreateScope())
                {

                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var feedbackService = new FeedbackService(context);

                    var result = await feedbackService.AddFeedback(new FeedbackDto() { Id = 1,  CustomerName = "Name1", FeedbackMessage = "Message1", DateSubmitted = new DateTime(2024, 11, 16, 11, 10, 14) });

                    var results = await feedbackService.GetAllFeedback();

                    Assert.Single(results);

                    Assert.Equal(feedback1.Id, results[0].Id);
                    Assert.Equal(feedback1.CustomerName, results[0].CustomerName);
                    Assert.Equal(feedback1.FeedbackMessage, results[0].FeedbackMessage);
                    //Assert.Equal(feedback1.DateSubmitted, results[0].DateSubmitted);

                }
            }
        }

        [Fact]
        public async void UpdateFeedback()
        {
            using (var provider = _services.BuildServiceProvider())
            {
                using (var scope = provider.CreateScope())
                {

                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    context.Feedback.Add(feedback1);

                    context.SaveChanges();

                    var feedbackService = new FeedbackService(context);


                    await feedbackService.UpdateFeedback(new FeedbackDto() { Id = 1, CustomerName = "Name2", FeedbackMessage = "Message2", DateSubmitted = new DateTime(2024, 11, 17, 12, 23, 24) });

                    var results = await feedbackService.GetAllFeedback();

                    Assert.Single(results);

                    Assert.Equal(feedback1.Id, 1);
                    Assert.Equal(feedback2.CustomerName, results[0].CustomerName);
                    Assert.Equal(feedback2.FeedbackMessage, results[0].FeedbackMessage);
                    //Assert.Equal(feedback2.DateSubmitted, results[0].DateSubmitted);

                }
            }
        }
    }
}