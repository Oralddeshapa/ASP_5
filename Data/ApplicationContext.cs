using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeaChair.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TeaChair.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { 
        }

        public DbSet<Teacher> Teachers { get; set;}

        public DbSet<Mark_for_numb> Grades { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<LogForDonate> Logs { get; set; }

    }
}
