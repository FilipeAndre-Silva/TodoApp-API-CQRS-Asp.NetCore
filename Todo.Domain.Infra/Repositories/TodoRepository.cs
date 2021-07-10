using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Domain.Infra.Contexts;
using Todo.Domain.Queries;
using Todo.Domain.Repositories;

namespace Todo.Domain.Infra.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DataContext _context;

        public TodoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> Create(TodoItem todo)
        {
            try
            {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
                return todo;
            }catch{
                return todo;
            }
        }

        public async Task<IEnumerable<TodoItem>> GetAll()
        {
            return await _context.Todos.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetAllDone()
        {
            return await _context.Todos
                .AsNoTracking()
                .Where(TodoQueries.GetAllDone())
                .OrderBy(x => x.Date).ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetAllUndone()
        {
            return await _context.Todos
                .AsNoTracking()
                .Where(TodoQueries.GetAllUndone())
                .OrderBy(x => x.Date).ToListAsync();
        }

        public async Task<TodoItem> GetById(Guid id)
        {
            return await _context
                .Todos
                .FirstOrDefaultAsync(TodoQueries.GetById(id));
        }

        public async Task<IEnumerable<TodoItem>> GetByPeriod(string user, DateTime date, bool done)
        {
            return await _context.Todos
                .AsNoTracking()
                .Where(TodoQueries.GetByPeriod(user, date, done))
                .OrderBy(x => x.Date).ToListAsync();
        }

        public async Task<TodoItem> Update(TodoItem todo)
        {
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return todo;
        }
    }
}