using TMPS.Domain.Interfaces;
using TMPS.Domain.Models;
using System.Text.Json;

namespace TMPS.DataAccess.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private const string FilePath = "AppData/employees.json";

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

        public Employee GetEmployeeById(int id)
        {
            List<Employee> employees = GetAllEmployees();
            return employees.FirstOrDefault(e => e.Id == id) ?? new Employee(0, "", "", 0);
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
            return new Employee(0, "", "", 0);
        }

        public void DeleteEmployee(int id)
        {
            List<Employee> employees = GetAllEmployees();
            employees.RemoveAll(e => e.Id == id);
            SaveEmployeesToJson(employees);
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
                // Handle the file save error, e.g., log it or throw an exception.
            }
        }
    }
}
