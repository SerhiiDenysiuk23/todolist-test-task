using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList_TestTask.Data;
using ToDoList_TestTask.Models;

namespace ToDoList_TestTask.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoListDBContext _context;

        public ToDoController(ToDoListDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ToDoSet.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDoSet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ToDo toDo)
        {
            try
            {
                toDo.Id = Guid.NewGuid();
                _context.Add(toDo);
                await _context.SaveChangesAsync();
            }catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDoSet.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }
            toDo.IsCompleted = !toDo.IsCompleted;

            try
            {
                _context.Update(toDo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoExists(toDo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Json(toDo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,IsCompleted")] ToDo toDo)
        {
            var item = await _context.ToDoSet.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.Title = toDo.Title;
            item.Description = toDo.Description;

            _context.ToDoSet.Update(item);
            _context.SaveChanges();

            return Json(item);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var toDo = await _context.ToDoSet.FindAsync(id);
            if (toDo != null)
            {
                _context.ToDoSet.Remove(toDo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoExists(Guid id)
        {
            return _context.ToDoSet.Any(e => e.Id == id);
        }
    }
}
