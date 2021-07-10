using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Domain.Entities;

namespace Todo.Domain.Repositories
{
   public interface ITodoRepository
    {
        Task<TodoItem> Create(TodoItem todo);
        Task<TodoItem> Update(TodoItem todo);
        Task<TodoItem> GetById(Guid id);
        Task<IEnumerable<TodoItem>> GetAll();
        Task<IEnumerable<TodoItem>> GetAllDone();
        Task<IEnumerable<TodoItem>> GetAllUndone();
        Task<IEnumerable<TodoItem>> GetByPeriod(string user, DateTime date, bool done);
    }
}