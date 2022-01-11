using Award.Core.Entities;
using System.Threading.Tasks;

namespace Award.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> CreateEmployee(ManasaEmployee manasaEmployee);
    }
}