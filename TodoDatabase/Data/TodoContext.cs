using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace TodoDatabase.Data
{
    public class TodoContext : DbContext
    {
        private readonly string _databasePath;

        public TodoContext(string databasePath)
        {
            _databasePath = databasePath;

            Database.EnsureCreated();
        }

        public DbSet<Job> Jobs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(string.Format("Filename={0}", _databasePath));
        }
    }
}
