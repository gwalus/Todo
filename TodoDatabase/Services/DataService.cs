using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Models;
using Todo.Services;
using TodoDatabase.Data;

namespace TodoDatabase.Services
{
    public class DataService : IDataService
    {
        private TodoSqliteContext _dbContext;

        public DataService(string dbPath)
        {
            _dbContext = new TodoSqliteContext(dbPath);
        }

        public async Task<List<Job>> GetJobs()
        {
            return await _dbContext.Jobs.ToListAsync();
        }

        public async Task<bool> AddJob<T>(T entity) where T : class
        {
            await _dbContext.AddAsync(entity);
            return await SaveChanged();

            //for (int i = 0; i < 1001; i++)
            //{
            //    var job = new Job()
            //    {
            //        Description = i.ToString(),
            //        Added = DateTime.Now.ToString(),
            //        IsEnded = true
            //    };

            //    await _dbContext.AddAsync(job);
            //    await _dbContext.SaveChangesAsync();
            //}

            //return true;
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

        public async Task<List<Job>> GetEndedJobs()
        {
            return await _dbContext.Jobs.Where(x => x.IsEnded).ToListAsync();
        }
    }
}
