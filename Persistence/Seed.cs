using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager) {
            if (!userManager.Users.Any() || context.Activities.Any()) {
                var users = new List<AppUser>
                {
                    new AppUser{DisplayName = "Bob", UserName = "bob", Email = "bob@bob.bob"},
                    new AppUser{DisplayName = "Tom", UserName = "tom", Email = "tom@tom.tom"},
                    new AppUser{DisplayName = "John", UserName = "john", Email = "john@john.john"}
                };

                foreach (var item in users)
                {
                    await userManager.CreateAsync(item, "Pa$$w0rd");
                }

                var activities = new List<Activity>
                {
                    new Activity
                    {
                        Title = "Activity 1",
                        Date = DateTime.Now.AddMonths(-2),
                        Description = "Desc 1",
                        Category = "drinks",
                        City = "City 1",
                        Venue = "Venue 1",
                        Attendees = new List<ActivityAttendee> { new ActivityAttendee{ AppUser = users[1] } }
                    },
                    new Activity
                    {
                        Title = "Activity 2",
                        Date = DateTime.Now.AddMonths(-1),
                        Description = "Desc 2",
                        Category = "food",
                        City = "City 2",
                        Venue = "Venue 2",
                        Attendees = new List<ActivityAttendee> { new ActivityAttendee{ AppUser = users[0] } }
                    },
                    new Activity
                    {
                        Title = "Activity 3",
                        Date = DateTime.Now.AddDays(-2),
                        Description = "Desc 3",
                        Category = "travel",
                        City = "City 3",
                        Venue = "Venue 3",
                        Attendees = new List<ActivityAttendee> { new ActivityAttendee{ AppUser = users[2] } }
                    }
                };

                await context.Activities.AddRangeAsync(activities);
                await context.SaveChangesAsync();
            }
        }
    }
}