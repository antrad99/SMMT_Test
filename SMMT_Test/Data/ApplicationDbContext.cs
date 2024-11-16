using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMMT_Test.Models;

namespace SMMT_Test.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Feedback> Feedback { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Feedback Table
            builder.Entity<Feedback>().ToTable("Feedback");
            builder.Entity<Feedback>().Property(x => x.FeedbackMessage).IsRequired().HasMaxLength(500);
            builder.Entity<Feedback>().Property(x => x.CustomerName).IsRequired().HasMaxLength(100);
            builder.Entity<Feedback>().Property(x => x.DateSubmitted).HasDefaultValue(DateTime.UtcNow);

            base.OnModelCreating(builder);
        }

    }
}
