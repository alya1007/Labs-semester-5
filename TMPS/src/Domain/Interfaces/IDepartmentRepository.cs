using TMPS.Domain.Models;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Interfaces
{
    public interface IDepartmentRepository
    {
        List<Department> GetDepartments();
        Department GetDepartmentByName(string departmentName);
        void AddEmployeeToDepartment(string departmentName, string employeeId);
        List<string> GetEmployeesIds(string departmentName);
    }
}