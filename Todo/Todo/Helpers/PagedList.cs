using System.Collections.Generic;
using System.Linq;
using Todo.Models;

namespace Todo.Helpers
{
    public class PagedList<T> : List<T>
    {
        #region Fields
        private readonly int _pageSize = 10;
        #endregion

        #region Methods
        public List<T> CreatePagedList(List<T> source, int currentPage)
        {
            return source
                .Skip((currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();
        }

        public List<Job> CreatePagedListByDescription(List<Job> source, int currentPage, string description)
        {
            return source
                  .Where(x => x.Description.StartsWith(description, System.StringComparison.OrdinalIgnoreCase))
                  .Skip((currentPage - 1) * _pageSize)
                  .Take(_pageSize)
                  .ToList();
        }

        public int GetJobsCountByDescription(List<Job> source, string description)
        {
            return source.Where(x => x.Description.StartsWith(description, System.StringComparison.OrdinalIgnoreCase)).Count();
        }
        #endregion
    }
}
