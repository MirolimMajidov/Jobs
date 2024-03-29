﻿using Jobs.Service.Common;
using JobService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobService.DataProvider
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(this JobsContext context, ILogger logger)
        {
            try
            {
                if (!context.Jobs.Any())
                {
                    await Task.Run(async () =>
                    {
                        var categories = new List<Category>()
                        {
                            new Category
                            {
                                Name = "Web, Mobile & Software Dev",
                                Description = "Web Development, Mobile Development, Desktop Software Developmen, QA & Testing",
                            },
                            new Category
                            {
                                Name = "Sales & Marketing",
                                Description = "Sales & Marketing Strategy",
                            },
                            new Category
                            {
                                Name = "Design & Writing",
                                Description = "Design, Writing, Photography & Translator",
                            },
                            new Category
                            {
                                Name = "Engineering & Architecture",
                                Description = "Engineering & Architecture",
                            }
                        };

                        await context.AddRangeAsync(categories);

                        var userId = Guid.NewGuid();
                        var userName = "Admin";
                        var jobs = new List<Job>()
                        {
                            new Job
                            {
                                Name = "Back-end developer",
                                Description = "ASP.Net Core and Xamarin developer",
                                Cost = 25,
                                Type = JobType.Hourly,
                                Duration = JobDuration.FromOneToThreeMonths,
                                CategoryId = categories[0].Id,
                                CreatedByUserId = userId,
                                CreatedByUserName = userName
                            },
                            new Job
                            {
                                Name = "Angular Developer Needed",
                                Description = "We need experienced Angular developer for short term project.",
                                Cost = 25,
                                Type = JobType.Hourly,
                                Duration = JobDuration.LessThanMonth,
                                CategoryId = categories[0].Id,
                                CreatedByUserId = userId,
                                CreatedByUserName = userName
                            },
                            new Job
                            {
                                Name = "Salesperson",
                                Description = "Salesperson needed",
                                Cost = 2000,
                                Type = JobType.FixedPrice,
                                Duration = JobDuration.FromOneToThreeMonths,
                                CategoryId = categories[1].Id,
                                CreatedByUserId = userId,
                                CreatedByUserName = userName
                            },
                            new Job
                            {
                                Name = "Design & Photography",
                                Description = "Design & Photography needed to build mockup of mobile app",
                                Cost = 30,
                                Type = JobType.Hourly,
                                Duration = JobDuration.MoreThanSixMonths,
                                CategoryId = categories[2].Id,
                                CreatedByUserId = userId,
                                CreatedByUserName = userName
                            }
                        };

                        await context.AddRangeAsync(jobs);

                        await context.SaveChangesAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception error while entering demo data: {Message}", ex.Message);
            }
        }
    }
}
