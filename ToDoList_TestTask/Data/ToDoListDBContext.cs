using Microsoft.EntityFrameworkCore;
using ToDoList_TestTask.Models;

namespace ToDoList_TestTask.Data
{
    public class ToDoListDBContext : DbContext
    {
        public DbSet<ToDo> ToDoSet { get; set; }

        public ToDoListDBContext(DbContextOptions options) : base(options) { }
        
    }
}
