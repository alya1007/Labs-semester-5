using TMPS.Domain.Interfaces;
using TMPS.Domain.Models;
using System.Text.Json;
using TMPS.Domain.Models.Abstractions;


namespace TMPS.DataAccess.Departments
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private const string FilePath = "src/AppData/departments.json";

        private static DepartmentRepository? _instance;

        private DepartmentRepository() { }

        public static DepartmentRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DepartmentRepository();
            }
            return _instance;
        }
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

        public void AddDepartment(Department department)
        {
            List<Department> departments = GetDepartments();
            departments.Add(department);
            SaveDepartmentsToJson(departments);
        }

        public void UpdateDepartment(Department department)
        {
            List<Department> departments = GetDepartments();
            Department departmentToUpdate = departments.FirstOrDefault(d => d.Name == department.Name) ?? throw new ArgumentException("Department not found.");
            departmentToUpdate = department;
            SaveDepartmentsToJson(departments);
        }

        public void RemoveDepartment(Department department)
        {
            List<Department> departments = GetDepartments();
            departments.Remove(department);
            SaveDepartmentsToJson(departments);
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