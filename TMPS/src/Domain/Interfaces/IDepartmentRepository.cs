using TMPS.Domain.Models;
using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Interfaces
{
    public interface IDepartmentRepository
    {

        public List<Department> GetDepartments();
        public Department GetDepartmentByName(string departmentName);
        public void AddDepartment(Department department);
        public void UpdateDepartment(Department department);
        public void RemoveDepartment(Department department);
    }
}