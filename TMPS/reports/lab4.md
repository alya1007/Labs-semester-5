# Topic: Behavioral Design Patterns

## Author: Alexandra Konjevic

### Group: FAF-213

## Objectives

- Study and understand the Behavioral Design Patterns.

- As a continuation of the previous laboratory work, think about what communication between software entities might be involved in your system.

- Create a new Project or add some additional functionalities using behavioral design patterns.

## Theory

Behavioral design patterns are a category of design patterns that focus on the interaction and communication between objects and classes. They provide a way to organize the behavior of objects in a way that is both flexible and reusable, while separating the responsibilities of objects from the specific implementation details. Behavioral design patterns address common problems encountered in object behavior, such as how to define interactions between objects, how to control the flow of messages between objects, or how to define algorithms and policies that can be reused across different objects and classes. Some examples from this category of design patterns are:

- Chain of Responsibility
- Command
- Interpreter
- Iterator
- Mediator
- Observer
- Strategy

## Main tasks

- Creating a new project or extending your last one project, implement at least 1 behavioral design pattern in your project:

  - The implemented design pattern should help to perform the tasks involved in your system.

  - The behavioral DPs can be integrated into you functionalities alongside the structural ones.

  - There should only be one client for the whole system.

- Keep your files grouped (into packages/directories) by their responsibilities (an example project structure):

  - client;

  - domain;

  - utilities;

  - data(if applies);

- Document the work in a separate markdown file according to the requirements.

## Implementation

### Iterator

Iterator is a behavioral design pattern that lets you traverse elements of a collection without exposing its underlying representation (list, stack, tree, etc.). The Iterator pattern allows clients to effectively loop over a collection of objects without knowing the specifics of that collection's implementation. This approach lets you separate algorithms from the data structure in which they operate. The main idea of the Iterator pattern is to extract the traversal behavior of a collection into a separate object called an iterator. The structure of this pattern is the following:

- `Iterator` - The Iterator interface declares the operations required for traversing a collection: fetching the next element, retrieving the current position, restarting iteration, etc. In my project, I use the C# built-in `IEnumerator` interface as an iterator and the `IEnumerable` interface as an aggregate:

- `Concrete Iterators` - Concrete Iterators implement specific algorithms for traversing a collection. The iterator object should track the traversal progress on its own. This allows several iterators to traverse the same collection independently of each other. In my project, I have two iterators, `Department` and `Team`. `Department` has a list of `Team` objects and `Team` has a list of `Employee` objects. The `Department` iterator traverses the list of `Team` objects and the `Team` iterator traverses the list of `Employee` objects:

```csharp
public class Department : IWorkUnit, IEnumerable<IWorkUnit>, IDepartmentEnumerator
    {
        public IWorkUnit Current { get; private set; } = null!;
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            // ...
        }

        public void Reset()
        {
            Current = null!;
        }

        public void Dispose() => Reset();

        public IEnumerator<IWorkUnit> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

```

```csharp
public class Team : IWorkUnit, IEnumerable<IWorkUnit>, ITeamEnumerator
    {
        public IWorkUnit Current { get; private set; } = null!;
        object IEnumerator.Current => Current;

        public IEnumerator<IWorkUnit> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool MoveNext()
        {
            // ...
        }

        public void Reset()
        {
            Current = null!;
        }

        public void Dispose() => Reset();
    }

```

- `Client` - The Client works with both collections and iterators via their interfaces. This way the client isn’t coupled to concrete classes, allowing you to use various collections and iterators with the same client code. Typically, clients don’t create iterators on their own, but instead get them from collections. Yet, in certain cases, the client can create one directly; for example, when the client defines its own special iterator. In my project, I use the iterators in the `Menu` class:

```csharp
private void GetDeveloperBySkill()
{
    Console.WriteLine("Enter skill: ");
    string skill = Console.ReadLine() ?? "";
    var departments = _departmentRepository.GetDepartments();
    foreach (var department in departments)
    {
        IEnumerator<IWorkUnit> departmentEnumerator = department.GetEnumerator();
        while (departmentEnumerator.MoveNext())
        {
            if (departmentEnumerator.Current is Team team)
            {
                IEnumerator<IWorkUnit> teamEnumerator = team.GetEnumerator();
                while (teamEnumerator.MoveNext())
                {
                    if (teamEnumerator.Current is Developer developer && developer.Skills.Contains(skill))
                    {
                        Console.WriteLine("Developer " + developer.Name + " has skill " + skill);
                    }
                }
            }
        }
    }
}

```

### Command

The command pattern is a behavioral design pattern that turns a request into a stand-alone object, encapsulating all the information about the request. This transformation lets you parameterize clients with queues, requests, and operations, and support undoable operations. The structure of this pattern is the following:

- `Command` - The Command interface declares a method for executing a command. In my project, I don’t implement this interface, because I use the C# built-in `Action` delegate as a command, when sending the commands to the invoker:

`private readonly List<Action> _commands;`.

- `Concrete Command` - Concrete Commands implement various kinds of requests. A concrete command isn’t supposed to perform the work on its own, but rather to pass the call to one of the business logic objects. However, for the sake of simplifying the code, these classes can be merged. In my project, I have many concrete commands, which are not separate classes, but methods in the `Menu` class:

```csharp

private void ListEmployees()
{
    // ...
}

private void AddEmployee()
{
    // ...
}

private void UpdateEmployee()
{
    // ...
}

// ...
```

- `Invoker` - The `Sender` class (aka invoker) is responsible for initiating requests. This class must have a field for storing a reference to a command object. The sender triggers that command instead of sending the request directly to the receiver. Note that the sender isn’t responsible for creating the command object. Usually, it gets a pre-created command from the client via the constructor. In my project, I use the `Menu` class as an invoker:

```csharp
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
        "Get Developer By Skill",
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
        GetDeveloperBySkill,
        () => Environment.Exit(0)
    };

    var menu = new MenuHandler(menuOptions, menuActions);
    menu.ShowMenu();
}
```

- `Receiver` - The Receiver class contains some business logic. Almost any object may act as a receiver. Most commands only handle the details of how a request is passed to the receiver, while the receiver itself does the actual work. In my project, I use the `MenuHandler` class as a receiver. This class is responsible for displaying the menu and executing the commands (the execution of the commands is handled by the package `ConsoleTools`):

```csharp
public class MenuHandler
{
    private readonly List<string> _titles;
    private readonly List<Action> _commands;

    public MenuHandler(List<string> titles, List<Action> commands)
    {
        _titles = titles;
        _commands = commands;
    }

    public void ShowMenu()
    {
        // ...
    }
}

```

### Strategy

Strategy is a behavioral design pattern that lets you define a family of algorithms, put each of them into a separate class, and make their objects interchangeable. The Strategy pattern suggests that you take a class that does something specific in a lot of different ways and extract all of these algorithms into separate classes called strategies. The original class, called context, must have a field for storing a reference to one of the strategies. The context delegates the work to a linked strategy object instead of executing it on its own. The structure of this pattern is the following:

- `Strategy` - The Strategy interface is common to all concrete strategies. It declares a method the context uses to execute a strategy. In my project, I use the `ISalaryCalculator` interface as a strategy:

```csharp
public interface ISalaryCalculator
{
    decimal CalculateSalary(Employee employee);
}
```

- `Context` - The Context maintains a reference to one of the concrete strategies and communicates with this object only via the strategy interface. In my project, I use the `Employee` class as a context. It has the field that behaves as a reference to the default strategy for calculating the Salary (`DefaultSalaryCalculator`):

```csharp
private ISalaryCalculator salaryCalculator = new DefaultSalaryCalculator();
```

Also, we can change the strategy at runtime by setting the value of the property `SalaryCalculator`:

```csharp
public ISalaryCalculator SalaryCalculator
{
    get => salaryCalculator;
    set => salaryCalculator = value ?? throw new ArgumentNullException(nameof(value));
}
```

And there is the method that uses the strategy to calculate the salary:

```csharp
public virtual decimal CalculateSalary()
{
    return salaryCalculator.CalculateSalary(this);
}

```

- `Concrete Strategies` - Concrete Strategies implement different variations of an algorithm the context uses. In my project, I have two concrete strategies: `DefaultSalaryCalculator` and `ManagerSalaryCalculator`:

```csharp
public class DefaultSalaryCalculator : ISalaryCalculator
{
    public decimal CalculateSalary(Employee employee)
    {
        return employee.BaseSalary + employee.BonusCoefficient * employee.BaseSalary;
    }
}

public class ManagerSalaryCalculator : ISalaryCalculator
{
    public decimal CalculateSalary(Employee employee)
    {
        if (employee is Manager manager && manager.TeamSize > 10) return employee.BaseSalary + employee.BonusCoefficient * employee.BaseSalary + 1000;
        else return employee.BaseSalary + employee.BonusCoefficient * employee.BaseSalary;
    }
}

```

- `Client` - The Client creates a specific strategy object and passes it to the context. The context exposes a setter which lets clients replace the strategy associated with the context at runtime. In my project, I use the `Menu` class as a client. It has the method that changes the strategy for calculating the salary:

```csharp
private void ListEmployees()
{
    var employees = _employeeRepository.GetAllEmployees();
    foreach (var employee in employees)
    {
        // ...
        if (employee is Manager)
        {
            employee.SalaryCalculator = new ManagerSalaryCalculator();
        }
        Console.WriteLine("Salary: " + employee.CalculateSalary());
        Console.WriteLine();
    }
}

```

## Conclusion

Behavioral design patterns in software engineering play a pivotal role in enhancing the flexibility, communication, and reusability of code by focusing on the interaction between objects and the delegation of responsibilities. These patterns aid in creating maintainable, extensible, and understandable software solutions by addressing various communication patterns among objects. The behavioral design patterns are concerned with the interaction and responsibility of objects. In my project, I used the Iterator, Command, and Strategy patterns. The Iterator pattern allowed me to traverse the collections of objects without exposing their underlying representation. The Command pattern allowed me to encapsulate the requests as objects and the Strategy pattern allowed me to define different strategies for calculating the salary of employees.
