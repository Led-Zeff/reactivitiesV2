using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context) {
            if (context.Activities.Any()) return;

            var activities = new List<Activity>
            {
                new Activity
                {
                    Title = "Activity 1",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Desc 1",
                    Category = "Category 1",
                    City = "City 1",
                    Venue = "Venue 1"
                },
                new Activity
                {
                    Title = "Activity 2",
                    Date = DateTime.Now.AddMonths(-1),
                    Description = "Desc 2",
                    Category = "Category 2",
                    City = "City 2",
                    Venue = "Venue 2"
                },
                new Activity
                {
                    Title = "Activity 3",
                    Date = DateTime.Now.AddDays(-2),
                    Description = "Desc 3",
                    Category = "Category 3",
                    City = "City 3",
                    Venue = "Venue 3"
                }
            };

            await context.Activities.AddRangeAsync(activities);
            await context.SaveChangesAsync();
        }
    }
}