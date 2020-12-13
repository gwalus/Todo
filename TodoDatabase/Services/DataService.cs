using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Models;
using Todo.Services;
using TodoDatabase.Data;

namespace TodoDatabase.Services
{
    public class DataService : IDataService
    {
        private TodoContext _dbContext;

        public DataService(string dbPath)
        {
            _dbContext = new TodoContext(dbPath);
        }

        public async Task<List<Job>> GetJobs()
        {
            return await _dbContext.Jobs.ToListAsync();
        }

        public async Task<bool> AddJob<T>(T entity) where T : class
        {
            await _dbContext.AddAsync(entity);
            return await SaveChanged();
        }

        public async Task<bool> UpdateJob(Job job)
        {
            _dbContext.Entry(await _dbContext.Jobs.FirstOrDefaultAsync(x => x.Id == job.Id)).CurrentValues.SetValues(job);
            return await SaveChanged();
        }

        public async Task<bool> SaveChanged()
        {
            if (await _dbContext.SaveChangesAsync() > 0)
                return true;
            return false;
        }
    }
}
