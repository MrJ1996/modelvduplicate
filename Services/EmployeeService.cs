//employeedata.cs//

using modelvduplicate.Models;
using modelvduplicate.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace modelvduplicate.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext data;

        public EmployeeService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await data.Employees
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await data.Employees.FindAsync(id);
        }


        public async Task<bool> IsEmailUniqueAsync(string email, int?
        ignoreId = null)
        {
            return !await data.Employees
                .AnyAsync(e => e.EmailAddress == email &&
                 (ignoreId == null || e.ID != ignoreId));
        }


        public async Task CreateAsync(Employee employee)
        {
            // Unique email validation
            if (await data.Employees.AnyAsync(e => e.EmailAddress == 
            employee.EmailAddress))
                throw new InvalidOperationException("Email already in use.");

            // Phone validation
            if (!string.IsNullOrEmpty(employee.PhoneNumber) && 
                employee.PhoneNumber.Length != 10)
                throw new InvalidOperationException
                    ("Phone number must be 10 digits.");

            data.Employees.Add(employee);
            await data.SaveChangesAsync();
        }


        public async Task UpdateAsync(Employee updated)
        {
            var employee = await data.Employees.FindAsync(updated.ID);
            if (employee == null) throw new KeyNotFoundException
                    ("Employee not found.");

            if (await data.Employees.AnyAsync
                (e => e.EmailAddress == updated.EmailAddress 
                && e.ID != updated.ID))
                throw new InvalidOperationException
                    ("Email already in use by another employee.");

            if (!string.IsNullOrEmpty(updated.PhoneNumber) 
                && updated.PhoneNumber.Length != 10)
                throw new InvalidOperationException
                    ("Phone number must be 10 digits.");

            employee.FirstName = updated.FirstName;
            employee.LastName = updated.LastName;
            employee.EmailAddress = updated.EmailAddress;
            employee.PhoneNumber = updated.PhoneNumber;

            await data.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var emp = await data.Employees.FindAsync(id);
            if (emp != null)
            {
                data.Employees.Remove(emp);
                await data.SaveChangesAsync();
            }
        }
    }
}