using System;
using System.Linq.Expressions;
using Todo.Domain.Entities;

namespace Todo.Domain.Queries
{
    public static class TodoQueries
    {
        public static Expression<Func<TodoItem, bool>> GetById(Guid id)
        {
            return x => x.Id == id;
        }

        public static Expression<Func<TodoItem, bool>> GetAll()
        {
            return x => x.User != null;
        }

        public static Expression<Func<TodoItem, bool>> GetAllDone()
        {
            return x => x.Done == true;
        }

        public static Expression<Func<TodoItem, bool>> GetAllUndone()
        {
            return x => x.Done == false;
        }

        public static Expression<Func<TodoItem, bool>> GetByPeriod(string user, DateTime date, bool done)
        {
            return x =>
                x.User == user &&
                x.Done == done &&
                x.Date.Date == date.Date;
        }
    }
}