using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test2.Models;

namespace Test2.Data
{
    public class Test2Context : DbContext
    {
        public Test2Context (DbContextOptions<Test2Context> options)
            : base(options)
        {
        }

        public DbSet<Test2.Models.Employee> Employee { get; set; } = default!;
    }
}
