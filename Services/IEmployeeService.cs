//iemployeeservice.cs//

using modelvduplicate.Models;
using System.Collections.Generic;   
using System.Threading.Tasks;

namespace modelvduplicate.Services
{
    public interface IEmployeeService
    {
        

        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email, int? ignoreId = null);
        Task CreateAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}
