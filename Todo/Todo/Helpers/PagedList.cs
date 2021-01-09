using System.Collections.Generic;
using System.Linq;
using Todo.Models;

namespace Todo.Helpers
{
    public class PagedList<T> : List<T>
    {
        public List<T> CreatePagedList(List<T> source, int currentPage, int pageSize = 10)
        {
            return source
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<Job> CreatePagedListByDescription(List<Job> source, int currentPage, string description, int pageSize = 10)
        {
            return source
                  .Where(x => x.Description == description)
                  .Skip((currentPage - 1) * pageSize)
                  .Take(pageSize)
                  .ToList();
        }
    }
}
