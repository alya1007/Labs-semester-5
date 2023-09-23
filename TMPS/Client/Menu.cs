using System;
using TMPS.Domain.Models;
using TMPS.Services;

namespace TMPS.Client
{
    public class Menu
    {
        private readonly EmployeeService _employeeService;

        public Menu(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public void Show()
        {
            Console.WriteLine("1. List all employees");
            Console.WriteLine("2. Add a new employee");
            Console.WriteLine("3. Update employee");
            Console.WriteLine("4. Delete employee");
            Console.WriteLine("5. Return to main menu");

            int choice = GetChoice();

            switch (choice)
            {
                case 1:
                    ListEmployees();
                    break;
                case 2:
                    AddEmployee();
                    break;
                case 3:
                    UpdateEmployee();
                    break;
                case 4:
                    DeleteEmployee();
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Show();
        }

        private int GetChoice()
        {
            Console.Write("Enter your choice: ");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                Console.Write("Enter your choice: ");
            }
            return choice;
        }

        private void ListEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            foreach (var employee in employees)
            {
                Console.WriteLine(employee);
            }
        }

        private void AddEmployee()
        {
            Console.Write("Enter employee name: ");
            string? name = Console.ReadLine();
            Console.Write("Enter employee position: ");
            string? position = Console.ReadLine();
            Console.Write("Enter employee salary: ");
            decimal salary;
            while (!decimal.TryParse(Console.ReadLine(), out salary))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                Console.Write("Enter employee salary: ");
            }
            _employeeService.AddEmployee(new Employee(name, position, salary));
        }

        private void UpdateEmployee()
        {
            Console.Write("Enter employee ID: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                Console.Write("Enter employee ID: ");
            }
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }
            Console.Write("Enter new employee name (leave blank to keep current value): ");
            string? name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                employee.Name = name;
            }
            Console.Write("Enter new employee position (leave blank to keep current value): ");
            string? position = Console.ReadLine();
            if (!string.IsNullOrEmpty(position))
            {
                employee.Position = position;
            }
            Console.Write("Enter new employee salary (leave blank to keep current value): ");
            decimal salary;
            if (decimal.TryParse(Console.ReadLine(), out salary))
            {
                employee.Salary = salary;
            }
            _employeeService.UpdateEmployee(employee);
        }

        private void DeleteEmployee()
        {
            Console.Write("Enter employee ID: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                Console.Write("Enter employee ID: ");
            }
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                Console.WriteLine("Employee not found.");
                return;
            }
            _employeeService.DeleteEmployee(employee);
        }
    }
}