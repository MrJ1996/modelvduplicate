using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using modelvduplicate.Models;
using modelvduplicate.Services;


namespace modelvduplicate.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService service; // ✅ CHANGED: _dbCont                                                                                             
        public EmployeesController(IEmployeeService service)
        {
            this.service = service ?? 
                throw new ArgumentNullException(nameof(service)); // ✅ CHANGED: service → service
        }

        // List all employees, ordered by last name
        public async Task<IActionResult> Index( )
        {
            var employees = await service.GetAllAsync(); // ✅ CHANGED: 
            return View(employees);
        }

        // Show details of one employee
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var employee = await service.GetByIdAsync(id.Value);

            if (employee == null) return NotFound();

            return View(employee);
        }

        // Show create form
        public IActionResult Create() 
            => View(new Employee());

        // Handle create post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid) return View(employee);

            try
            {
                await service.CreateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employee);
            }
        }

        // Show edit form
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var employee = await service.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // Handle edit post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Employee updated)
        {
            if (id != updated.ID) return BadRequest();
            if (!ModelState.IsValid) return View(updated);

            try
            {
                await service.UpdateAsync(updated);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Show delete confirmation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteAsync(id); // service handles logic
            return RedirectToAction(nameof(Index));
        }
    }
}

