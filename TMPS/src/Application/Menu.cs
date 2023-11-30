using TMPS.Common;
using TMPS.Domain.Interfaces;
using TMPS.Domain.Models;
using TMPS.Domain.Models.Abstractions;
using TMPS.Domain.Models.Reports;
using TMPS.UseCases.Employees;

namespace TMPS.Application;

public class Menu
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeFactory _employeeFactory;
    private const string DEVELOPER = "Developer";
    private const string MANAGER = "Manager";
    private const string HRMANAGER = "HRManager";
    private readonly HashSet<string> _employeeTypes = new() { DEVELOPER, MANAGER, HRMANAGER };
    private readonly Dictionary<string, Func<string, decimal, Employee>> _employeeFactories = new();
    private readonly Dictionary<string, Action> _employeeRefiners = new();
    private Employee? _employee;

    public Menu(IEmployeeFactory employeeFactory, IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
    {
        _employeeFactory = employeeFactory;
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;

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
        var menuOptions = new List<string>()
        {
            "List Employees",
            "Add Employee",
            "Update Employee",
            "Delete Employee",
            "Print Employee Report",
            "List All Departments",
            "Add Department",
            "Remove Team from Department",
            "Add Team to Department",
            "Calculate Salary of Department",
            "Exit"
        };

        var menuActions = new List<Action>()
        {
            ListEmployees,
            AddEmployee,
            UpdateEmployee,
            DeleteEmployee,
            GetEmployeeReport,
            ListDepartments,
            AddDepartment,
            RemoveTeamFromDepartment,
            AddTeamToDepartment,
            CalculateSalaryOfDepartment,
            () => Environment.Exit(0)
        };

        var menu = new MenuHandler(menuOptions, menuActions);
        menu.ShowMenu();
    }
    private void ListDepartments()
    {
        var departments = _departmentRepository.GetDepartments();
        foreach (var department in departments)
        {
            System.Console.WriteLine("Department " + department.Name);
        }
    }

    private void CalculateSalaryOfDepartment()
    {
        System.Console.WriteLine("Enter department name: ");
        string departmentName = Console.ReadLine() ?? "";
        Department department = _departmentRepository.GetDepartmentByName(departmentName);

        System.Console.WriteLine("Total salary of department: " + department.CalculateSalary());
    }

    private void AddTeamToDepartment()
    {
        System.Console.WriteLine("Enter department name: ");
        string departmentName = Console.ReadLine() ?? "";
        Department department = _departmentRepository.GetDepartmentByName(departmentName);

        var team = CreateTeam();
        department.AddTeam(team);

        _departmentRepository.UpdateDepartment(department);
    }

    private void RemoveTeamFromDepartment()
    {
        System.Console.WriteLine("Enter department name: ");
        string departmentName = Console.ReadLine() ?? "";
        Department department = _departmentRepository.GetDepartmentByName(departmentName);

        System.Console.WriteLine("Enter team name: ");
        string teamName = Console.ReadLine() ?? "";
        department.RemoveTeam(teamName);

        _departmentRepository.UpdateDepartment(department);
    }

    private void AddDepartment()
    {
        System.Console.WriteLine("Enter department name: ");
        string departmentName = Console.ReadLine() ?? "";
        Department department = new Department(departmentName);

        bool addTeam = true;

        while (addTeam)
        {
            System.Console.WriteLine("Add team? (y/n): ");
            string choice = Console.ReadLine() ?? "";
            addTeam = choice == "y" ? true : false;
            if (addTeam)
            {
                var team = CreateTeam();
                department.AddTeam(team);
            }
        }

        _departmentRepository.AddDepartment(department);

    }

    private IWorkUnit CreateTeam()
    {
        System.Console.WriteLine("Enter team name: ");
        string teamName = Console.ReadLine() ?? "";
        Team team = new Team(teamName);

        bool addEmployee = true;
        while (addEmployee)
        {
            System.Console.WriteLine("Add employee? (y/n): ");
            string choice = Console.ReadLine() ?? "";
            addEmployee = choice == "y" ? true : false;
            if (addEmployee)
            {
                var employeeType = GetEmployeeType();
                var employee = CreateEmployee(employeeType);
                team.Employees.Add(employee);
            }
        }

        return team;
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
        var employeeType = GetEmployeeType();
        var employee = CreateEmployee(employeeType);
        _employeeRepository.AddEmployee(employee);
    }

    private string GetEmployeeType()
    {
        Console.Write("Enter employee type (Developer, Manager, HRManager): ");
        string employeeType = Console.ReadLine() ?? "";
        while (string.IsNullOrEmpty(employeeType) || !_employeeTypes.Contains(employeeType))
        {
            Console.WriteLine("Invalid employee type.");
            Console.Write("Enter employee type (Developer, Manager, HRManager): ");
            employeeType = Console.ReadLine() ?? "";
        }
        return employeeType;
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