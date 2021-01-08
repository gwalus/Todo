using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Models;

namespace Todo.Services
{
    public interface IDataService
    {
        Task<List<Job>> GetJobs();
        Task<List<Job>> GetEndedJobs();
        Task<bool> AddJob<T>(T entity) where T : class;
        Task<bool> UpdateJob(Job job);
        Task<bool> SaveChanged();
    }
}
