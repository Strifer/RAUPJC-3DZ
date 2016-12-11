using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadatak1.Models;

namespace Zadatak1.Database
{
    public class TodoDbContext : DbContext
    {
        public IDbSet<TodoItem> TodoItems { get; set; }

        public TodoDbContext(string connection) : base(connection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasKey(c => c.Id);
            modelBuilder.Entity<TodoItem>().Property(c => c.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(c => c.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(c => c.IsCompleted).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(c => c.DateCompleted).IsOptional();
        }
    }
}
