using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadatak1.Interfaces;
using Zadatak1.Models;

namespace Zadatak1.Database
{
    public class TodoSqlRepository : ITodoRepository
    {

        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public void Add(TodoItem todoItem)
        {
            if (todoItem == null)
            {
                throw new ArgumentNullException();
            }
            TodoItem i = _context.TodoItems.Where(x => x.Id == todoItem.Id).FirstOrDefault();
            if (i != null)
            {
                throw new DuplicateTodoItemException("duplicate id: {" + i.Id + "}");
            }
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            TodoItem i = _context.TodoItems.Where(x => x.Id == todoId).FirstOrDefault();
            if (i.UserId != userId)
            {
                throw new TodoAccessDeniedException("This user does not have access to this file. Incompatible userIds.");
            }
            return i;
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(x => x.UserId == userId && !x.IsCompleted).ToList();
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(x => x.UserId == userId).OrderByDescending(x => x.DateCreated).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(x => x.UserId == userId && x.IsCompleted).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(x => x.UserId == userId && filterFunction(x)).ToList();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem item = _context.TodoItems.Where(x => x.Id == todoId).FirstOrDefault();
            if (item == null)
            {
                return false;
            }

            if (item.UserId != userId)
            {
                throw new TodoAccessDeniedException("This user does not have access to this file. Incompatible userIds.");
            }
            item.MarkAsCompleted();
            _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem item = _context.TodoItems.Where(x => x.Id == todoId).FirstOrDefault();
            if (item == null)
            {
                return false;
            }

            if (item.UserId != userId)
            {
                throw new TodoAccessDeniedException("This user does not have access to this file. Incompatible userIds.");
            }
            _context.TodoItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            TodoItem item = _context.TodoItems.Where(x => x.Id == todoItem.Id).FirstOrDefault();
            if (item == null)
            {
                this.Add(todoItem);
            }

            if (item.UserId != userId)
            {
                throw new TodoAccessDeniedException("This user does not have access to this file. Incompatible userIds.");
            }
            item = todoItem;
            _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
