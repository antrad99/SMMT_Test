using Microsoft.EntityFrameworkCore;
using SMMT_Test.Models;

namespace SMMT_Test.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                //Seed the db in here
                if (!context.Feedback.Any())
                {
                    context.Feedback.Add(new Feedback
                    {
                        CustomerName = "Luca",
                        FeedbackMessage = "Hello!",
                        DateSubmitted = DateTime.UtcNow
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
