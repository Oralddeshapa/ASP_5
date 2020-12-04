using System;
using System.Collections.Generic;
using System.Linq;
using TeaChair.Data;
using TeaChair.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace TeaChair.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationContext>>()))
            {
                if (context.Teachers.Any())
                {
                    return;   
                }

                context.Teachers.AddRange(
                    new Teacher
                    {
                        Name = "Kalugina",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Subject = "Math",
                        Tier = "SSS"
                    },

                    new Teacher
                    {
                        Name = "Disco-Shuman ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Subject = "Philosophy",
                        Tier = "A"
                    },

                    new Teacher
                    {
                        Name = "Anisimov",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Subject = "Comedy",
                        Tier = "C"
                    }

                );
                context.SaveChanges();
            }
        }
    }
}
