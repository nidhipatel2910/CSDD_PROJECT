using Microsoft.EntityFrameworkCore;
using Test2.Models;

namespace Test2.Data
{
    public class Test2Context : DbContext
    {
        public Test2Context(DbContextOptions<Test2Context> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<InvitationHistory> InvitationHistory { get; set; }
    }
}
