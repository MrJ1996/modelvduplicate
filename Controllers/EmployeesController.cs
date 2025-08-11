using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using modelvduplicate.Data;
using modelvduplicate.Models;

namespace modelvduplicate.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext database;

        public EmployeesController(ApplicationDbContext context)
        {
            database = context;
        }

        public async Task<IActionResult> Index()
        {
            var allEmployees = await database.Employees.ToListAsync();
            return View(allEmployees);
        }

        public async Task<IActionResult> Details(int? employeeId)
        {
            if (employeeId == null) return NotFound();

            var foundEmployee = await database.Employees
                .Where(emp => emp.ID == employeeId)
                .FirstOrDefaultAsync();

            if (foundEmployee == null) return NotFound();

            return View(foundEmployee);
        }

        public IActionResult Create()
        {
            var newEmployee = new Employee();
            return View(newEmployee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employeeData)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeData);
            }

            database.Employees.Add(employeeData);
            var result = await database.SaveChangesAsync();

            if (result > 0)
            {
                return RedirectToAction("Index");
            }

            return View(employeeData);
        }

        public async Task<IActionResult> Edit(int? employeeId)
        {
            if (employeeId == null) return NotFound();

            var employeeToEdit = await database.Employees.FindAsync(employeeId);

            if (employeeToEdit == null) return NotFound();

            return View(employeeToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int employeeId, Employee updatedData)
        {
            if (employeeId != updatedData.ID) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(updatedData);
            }

            try
            {
                database.Entry(updatedData).State = EntityState.Modified;
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                var employeeStillExists = await database.Employees
                    .AnyAsync(e => e.ID == updatedData.ID);

                if (!employeeStillExists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IActionResult> Delete(int? employeeId)
        {
            if (employeeId == null) return NotFound();

            var employeeToDelete = await database.Employees
                .Where(emp => emp.ID == employeeId)
                .FirstOrDefaultAsync();

            if (employeeToDelete == null) return NotFound();

            return View(employeeToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int employeeId)
        {
            var targetEmployee = await database.Employees.FindAsync(employeeId);

            if (targetEmployee != null)
            {
                database.Employees.Remove(targetEmployee);
                await database.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}