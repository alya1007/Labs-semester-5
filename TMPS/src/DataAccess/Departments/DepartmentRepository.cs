using TMPS.Domain.Interfaces;
using TMPS.Domain.Models;
using System.Text.Json;


namespace TMPS.DataAccess.Departments
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private const string FilePath = "src/AppData/departments.json";
        public List<Department> GetDepartments()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return new List<Department>();
                }

                List<Department>? departments;
                using (StreamReader file = File.OpenText(FilePath))
                {
                    departments = JsonSerializer.Deserialize<List<Department>>(file.ReadToEnd());
                }

                return departments ?? new List<Department>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                return new List<Department>();
            }
        }

        public Department GetDepartmentByName(string departmentName)
        {
            List<Department> departments = GetDepartments();
            return departments.FirstOrDefault(d => d.Name == departmentName) ?? throw new ArgumentException("Department not found.");
        }

        public void AddEmployeeToDepartment(string departmentName, string employeeId)
        {
            var departments = GetDepartments();
            var department = departments.FirstOrDefault(d => d.Name == departmentName);
            if (department == null)
            {
                throw new ArgumentException("Department not found.");
            }
            department.EmployeesIds.Add(employeeId);
            var index = departments.FindIndex(d => d.Name == departmentName);
            departments[index] = department;
            SaveDepartmentsToJson(departments);
        }

        public List<string> GetEmployeesIds(string departmentName)
        {
            var departments = GetDepartments();
            var department = departments.FirstOrDefault(d => d.Name == departmentName);
            if (department == null)
            {
                throw new ArgumentException("Department not found.");
            }
            return department.EmployeesIds;
        }

        private void SaveDepartmentsToJson(List<Department> departments)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(departments);
                File.WriteAllText(FilePath, jsonString);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
            }
        }
    }
}