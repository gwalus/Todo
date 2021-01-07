using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace TodoDatabase.Data
{
    public class TodoSqliteContext : DbContext
    {
        private readonly string _databasePath;

        public TodoSqliteContext(string databasePath)
        {
            _databasePath = databasePath;

            Database.EnsureCreated();
        }

        public DbSet<Job> Jobs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source = {_databasePath}");
        }
    }
}
