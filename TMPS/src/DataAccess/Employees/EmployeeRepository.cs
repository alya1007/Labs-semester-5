using TMPS.Domain.Interfaces;
using TMPS.Domain.Models.Abstractions;
using System.Text.Json;

namespace TMPS.DataAccess.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private const string FilePath = "src/AppData/employees.json";

        public List<Employee> GetAllEmployees()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return new List<Employee>();
                }

                List<Employee>? employees;
                using (StreamReader file = File.OpenText(FilePath))
                {
                    employees = JsonSerializer.Deserialize<List<Employee>>(file.ReadToEnd());
                }

                return employees ?? new List<Employee>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                return new List<Employee>();
            }
        }

        public Employee GetEmployeeById(string id)
        {
            Guid guidId = Guid.Parse(id);
            List<Employee> employees = GetAllEmployees();
            return employees.FirstOrDefault(e => e.Id == guidId) ?? throw new ArgumentException("Employee not found.");
        }

        public Employee AddEmployee(Employee employee)
        {
            List<Employee> employees = GetAllEmployees();
            employees.Add(employee);
            SaveEmployeesToJson(employees);
            return employee;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            List<Employee> employees = GetAllEmployees();
            int index = employees.FindIndex(e => e.Id == employee.Id);
            if (index != -1)
            {
                employees[index] = employee;
                SaveEmployeesToJson(employees);
                return employee;
            }
            throw new ArgumentException("Employee not found.");
        }

        public void DeleteEmployee(string id)
        {
            List<Employee> employees = GetAllEmployees();
            int index = employees.FindIndex(e => e.Id == Guid.Parse(id));
            if (index != -1)
            {
                employees.RemoveAt(index);
                SaveEmployeesToJson(employees);
                return;
            }
            throw new ArgumentException("Employee not found.");

        }

        private void SaveEmployeesToJson(List<Employee> employees)
        {
            try
            {
                string json = JsonSerializer.Serialize(employees);
                File.WriteAllText(FilePath, json);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving JSON to file: {ex.Message}");
            }
        }
    }
}
