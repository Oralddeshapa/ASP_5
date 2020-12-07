using System;
using System.Collections.Generic;
using System.Linq;
using TeaChair.Data;
using TeaChair.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TeaChair.Controllers;

namespace TeaChair.Models
{
    public class SeedData
    {
        public static async void Initialize(IServiceProvider serviceProvider, UserManager<User> userManager, ILogger<SeedData> logger, RoleManager<IdentityRole> roleManager)
        {
            Random rand = new Random();

            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationContext>>()))
            {
                if (context.Teachers.Any())
                {

                }
                else
                {
                    context.Teachers.AddRange(
                        new Teacher
                        {
                            Name = "Kalugina",
                            ReleaseDate = DateTime.Parse("1989-2-12"),
                            Subject = "Math",
                            Tier = "SS",
                            points = 4010
                        },

                        new Teacher
                        {
                            Name = "Disco-Shuman ",
                            ReleaseDate = DateTime.Parse("1984-3-13"),
                            Subject = "Philosophy",
                            Tier = "A",
                            points = 500
                        },

                        new Teacher
                        {
                            Name = "Anisimov",
                            ReleaseDate = DateTime.Parse("1986-2-23"),
                            Subject = "Comedy",
                            Tier = "C",
                            points = 100
                        }

                    );
                    context.SaveChanges();
                }
                if (context.Classes.Any())
                {

                }
                else
                {
                    context.Classes.AddRange(
                        new Class
                        {
                            Tier = "SS",
                            Min = 4001,
                            Max = 8000
                        },

                        new Class
                        {
                            Tier = "S",
                            Min = 601,
                            Max = 4000
                        },

                        new Class
                        {
                            Tier = "A",
                            Min = 401,
                            Max = 600
                        },

                        new Class
                        {
                            Tier = "B",
                            Min = 201,
                            Max = 400
                        },

                        new Class
                        {
                            Tier = "C",
                            Min = 1,
                            Max = 200
                        },

                        new Class
                        {
                            Tier = "D",
                            Min = -4000,
                            Max = 0
                        }

                    );
                    context.SaveChanges();
                }
                if (context.Grades.Any())
                {

                }
                else
                {


                    for (int i = 0; i < 20; i++)
                        context.Grades.AddRange(
                            new Mark_for_numb
                            {
                                Number = 85350100 + i,
                                Points = rand.Next(0, 100)
                            }
                        );

                    context.Grades.AddRange(
                        new Mark_for_numb
                        {
                            Number = 85350155,
                            Points = 1000
                        }
                    );

                    context.SaveChanges();
                }
            }
            /*if (context.Users.Any())
            {

            }
            else
            {*/
            /*logger.LogDebug(2, "Admin Kappa ----------------------------------------------");
                User admin = new User
            {
                Points = 85350155,
                Email = "Orald" + "@bsuir.by",
                UserName = "Orald"
            };
            await _userManager.CreateAsync(admin, "123456");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "user");
                await _userManager.AddToRoleAsync(admin, "moder");
                await _userManager.AddToRoleAsync(admin, "admin");
            }

            for (int i = 0; i < 20; i++)
            {
                int tom = 85350100 + i;
                string tim = tom.ToString();
                User user_1 =
                    new User
                    {
                        Points = 85350100 + i,
                        Email = tim + "@bsuir.by",
                        UserName = tim
                    };
                int pass = rand.Next(100000, 999999);
                result = await _userManager.CreateAsync(user_1, pass.ToString());
                await _userManager.AddToRoleAsync(user_1, "admin");
            }*/
            //context.SaveChanges();
            //}

        }
    }
}



/*
  Random rand = new Random();
                User admin = new User
                {
                    Points = 85350155,
                    Email = "Orald" + "@bsuir.by",
                    UserName = "Orald"
                };
                await _userManager.CreateAsync(admin, "123456");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "user");
                    await _userManager.AddToRoleAsync(admin, "moder");
                    await _userManager.AddToRoleAsync(admin, "admin");
                }

                for (int i = 0; i < 20; i++)
                {
                    int tom = 85350100 + i;
                    string tim = tom.ToString();
                    User user_1 =
                        new User
                        {
                            Points = 85350100 + i,
                            Email = tim + "@bsuir.by",
                            UserName = tim
                        };
                    int pass = (85350100 + i) / (i + 1) * i;
                    result = await _userManager.CreateAsync(user_1, pass.ToString());
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user_1, "user");
                    }
                }
                
 */