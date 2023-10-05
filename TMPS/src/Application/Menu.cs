using TMPS.Common;
using TMPS.DataAccess.Employees;
using TMPS.Domain.Factory;
using TMPS.Domain.Interfaces;
using TMPS.Domain.Models;
using TMPS.Domain.Models.Abstractions;
using TMPS.Domain.Models.Reports;
using TMPS.UseCases.Employees;

namespace TMPS.Application;

public class Menu
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeFactory _employeeFactory;
    private const string DEVELOPER = "Developer";
    private const string MANAGER = "Manager";
    private const string HRMANAGER = "HRManager";
    private readonly HashSet<string> _employeeTypes = new() { DEVELOPER, MANAGER, HRMANAGER };
    private readonly Dictionary<string, Func<string, decimal, Employee>> _employeeFactories = new();
    private readonly Dictionary<string, Action> _employeeRefiners = new();
    private Employee? _employee;

    public Menu(IEmployeeFactory employeeFactory, IEmployeeRepository employeeRepository)
    {
        _employeeFactory = employeeFactory;
        _employeeRepository = employeeRepository;

        _employeeFactories.Add(DEVELOPER, _employeeFactory.CreateDeveloper);
        _employeeFactories.Add(MANAGER, _employeeFactory.CreateManager);
        _employeeFactories.Add(HRMANAGER, _employeeFactory.CreateHRManager);

        _employeeRefiners.Add(DEVELOPER, RefineDeveloper);
        _employeeRefiners.Add(MANAGER, RefineManager);
        _employeeRefiners.Add(HRMANAGER, RefineHRManager);

    }

    private Employee CreateEmployee(string employeeType)
    {
        string name = "";
        decimal baseSalary = 0;

        while (!_employeeTypes.Contains(employeeType))
        {
            Console.WriteLine("Invalid employee type.");
            Console.Write("Enter employee type (Developer, Manager, HRManager): ");
            employeeType = Console.ReadLine() ?? "";
        }
        Console.WriteLine("Enter name: ");
        name = Console.ReadLine() ?? "";

        Console.WriteLine("Enter base salary: ");
        while (!decimal.TryParse(Console.ReadLine(), out baseSalary))
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }

        _employee = _employeeFactories[employeeType](name, baseSalary);
        _employeeRefiners[employeeType]();

        return _employee;
    }

    private void RefineDeveloper()
    {
        Console.Write("Enter skills (comma separated): ");
        var skills = (Console.ReadLine() ?? "").Split(',').Select(s => s.Trim()).ToList()!;
        if (_employee is Developer developer)
        {
            developer.Skills = skills;
        }
    }

    private void RefineManager()
    {
        int teamSize;
        Console.Write("Enter team size: ");
        while (!int.TryParse(Console.ReadLine(), out teamSize))
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
        if (_employee is Manager manager)
        {
            manager.TeamSize = teamSize;
        }
    }

    private void RefineHRManager()
    {
        Console.Write("Enter department: ");
        string department = Console.ReadLine() ?? "";
        if (_employee is HRManager hrManager)
        {
            hrManager.Department = department;
        }
    }

    public void Show()
    {
        bool exitMenu = false;

        while (!exitMenu)
        {
            Console.WriteLine();
            Console.WriteLine("Employee Management Menu");
            Console.WriteLine("1. List Employees");
            Console.WriteLine("2. Add Employee");
            Console.WriteLine("3. Update Employee");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Print Employee Report");
            Console.WriteLine("6. Exit");

            int choice = Convert.ToInt32(Console.ReadLine());

            OptionsHandler<int> menuOptionsHandler = new OptionsHandler<int>()
                .AddOption(1, ListEmployees)
                .AddOption(2, AddEmployee)
                .AddOption(3, UpdateEmployee)
                .AddOption(4, DeleteEmployee)
                .AddOption(5, GetEmployeeReport)
                .AddOption(6, () => exitMenu = true)
                .SetDefaultFallback(() => Console.WriteLine("Invalid choice."));

            menuOptionsHandler.HandleOption(choice);
        }
    }

    private void GetEmployeeReport()
    {
        Console.WriteLine();
        Console.WriteLine("1. Default report");
        Console.WriteLine("2. Detailed report");
        Console.WriteLine("3. Progress report");

        int choice = Convert.ToInt32(Console.ReadLine());

        OptionsHandler<int> reportTypesHandler = new OptionsHandler<int>()
            .AddOption(1, () =>
            {
                Console.WriteLine("1. Default report");
                Report report = ReportPrototypes.DefaultReport(GetEmployeeForReport()).Clone();
                report.Print();
            })
            .AddOption(2, () =>
            {
                Console.WriteLine("2. Detailed report");
                Report report = ReportPrototypes.DetailedReport(GetEmployeeForReport()).Clone();
                report.Print();
            })
            .AddOption(3, () =>
            {
                Console.WriteLine("3. Progress report");
                Report report = ReportPrototypes.ProgressReport(GetEmployeeForReport()).Clone();
                report.Print();
            })
            .SetDefaultFallback(() => Console.WriteLine("Invalid choice."));

        reportTypesHandler.HandleOption(choice);
    }

    private Employee GetEmployeeForReport()
    {
        Console.Write("Employee id: ");
        string id = Console.ReadLine() ?? "";
        Console.WriteLine();
        var employee = _employeeRepository.GetEmployeeById(id);
        return employee;
    }


    private void ListEmployees()
    {
        var employees = _employeeRepository.GetAllEmployees();
        foreach (var employee in employees)
        {
            Console.WriteLine("Employee " + employee.Id.ToString());
            Console.WriteLine("Name: " + employee.Name);
            Console.WriteLine("Position: " + employee.GetType().Name);
            Console.WriteLine("Base salary: " + employee.BaseSalary);
            Console.WriteLine("Bonus coefficient: " + employee.BonusCoefficient);
            Console.WriteLine("Salary: " + SalaryCalculator.CalculateSalary(employee));
            Console.WriteLine();
        }
    }

    private void AddEmployee()
    {
        Console.Write("Enter employee type (Developer, Manager, HRManager): ");
        string employeeType = Console.ReadLine() ?? "";
        while (string.IsNullOrEmpty(employeeType) || !_employeeTypes.Contains(employeeType))
        {
            Console.WriteLine("Invalid employee type.");
            Console.Write("Enter employee type (Developer, Manager, HRManager): ");
            employeeType = Console.ReadLine() ?? "";
        }
        var employee = CreateEmployee(employeeType);
        _employeeRepository.AddEmployee(employee);
    }

    private void UpdateEmployee()
    {
        Console.Write("Enter employee ID: ");
        string id = Console.ReadLine() ?? "";

        Console.WriteLine("1. Update name");
        Console.WriteLine("2. Update base salary");
        Console.WriteLine("3. Update bonus coefficient");

        string newName = "";
        decimal newBaseSalary = 0;
        decimal newBonusCoefficient = 0;

        int choice = Convert.ToInt32(Console.ReadLine());

        OptionsHandler<int> updateOptionsHandler = new OptionsHandler<int>()
            .AddOption(1, () => { Console.Write("Enter new name: "); newName = Console.ReadLine() ?? ""; })
            .AddOption(2, () => { Console.Write("Enter new base salary: "); newBaseSalary = Convert.ToDecimal(Console.ReadLine()); })
            .AddOption(3, () => { Console.Write("Enter new bonus coefficient: "); newBonusCoefficient = Convert.ToDecimal(Console.ReadLine()); })
            .SetDefaultFallback(() => Console.WriteLine("Invalid choice."));

        updateOptionsHandler.HandleOption(choice);

        var employee = _employeeRepository.GetEmployeeById(id);
        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        employee.Name = newName == "" ? employee.Name : newName;
        employee.BaseSalary = newBaseSalary == 0 ? employee.BaseSalary : newBaseSalary;
        employee.BonusCoefficient = newBonusCoefficient == 0 ? employee.BonusCoefficient : newBonusCoefficient;

        _employeeRepository.UpdateEmployee(employee);

    }

    private void DeleteEmployee()
    {
        Console.WriteLine("Enter employee ID: ");
        string id = Console.ReadLine() ?? "";
        _employeeRepository.DeleteEmployee(id);
    }

}