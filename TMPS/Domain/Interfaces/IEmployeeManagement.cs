using TMPS.Domain.Models;

namespace TMPS.Domain.Interfaces
{
    public interface IEmployeeManagement
    {
        void AddEmployee(Employee employee);
        List<Employee> GetAllEmployees();
    }
}
