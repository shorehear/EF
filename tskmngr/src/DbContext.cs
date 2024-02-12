using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace tskmngr
{
    public class TodoDbContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=todo.db");
        }
    }
}