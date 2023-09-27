# Topic: SOLID Principles

## Author: Alexandra Konjevic

### Group: FAF-213

## Objectives

- Study and understand the SOLID Principles

- Choose a domain, define its main classes/models/entities and choose the appropriate instantiation mechanisms

- Create a sample project that respects SOLID Principles

## Theory

The SOLID principles are a set of five design principles in object-oriented programming (OOP) that serve as a guide to writing maintainable and scalable software. These principles were introduced by Robert C. Martin and have become fundamental concepts for software developers seeking to create robust and flexible code. Each letter in "SOLID" represents a different principle: Single responsibility, Open-closed, Liskov substitution, Interface segregation and Dependency inversion.

## Main tasks

- Choose an OO programming language and a suitable IDE or Editor (No frameworks/libs/engines allowed)

- Select a domain area for the sample project

- Define the main involved classes and think about what instantiation mechanisms are needed

- Respect SOLID Principles in your project

## Implementation

I created an employee management console application in C#. The structure of the project is as follows:

- `Domain`

    - `Models` folder contains the classes that represent the entities of the application: `Developer`, `Manager` and `HRManager`. It also has an `Abstractions` directory that contains the `Employee` class, which is the base class for all the other entities.

    - `Factory` folder contains the `EmployeeFactory.cs` class, which is responsible for creating the entities (methods `CreateDeveloper`, `CreateManager` and `CreateHRManager`).

    - `Interfaces` folder contains the `IEmployeeFactory` and `IEmployeeRepository` interfaces.

- `UseCases`
    
    - `Employees\SalaryCalculator.cs` is responsible for calculating the salary of an employee, using the `BaseSalary` and `BonusCoefficient` properties of the employee.

- `AppData`

    - `employees.json` is the database of the application, which contains the employees.

- `DataAccess`

    - `Employees\EmployeeRepository.cs` is responsible for interacting with the database (`employees.json`). It has the following methods: `GetAllEmployees`, `GetEmployeeById`, `AddEmployee`, `UpdateEmployee`, `DeleteEmployee`, `SaveEmployeesToJson` (this method serialize the data in JSON format).

- `Client` folder contains the `Program.cs` file, which is the entry point of the application.

- `Application`

    - `Menu.cs` is responsible for displaying the menu of the application and handling the user input. It can realize different functions: displaying all the employees, adding a new employee, updating an employee, deleting an employee.

- `Common`

    - `OptionsHandler.cs` is a builder class that is responsible for handling the user input and creating the options for the menu.

## Solid Principles

#### Single Responsibility Principle

The Single Responsibility Principle states that a class should have only one reason to change. In my project:

- The `EmployeeRepository` class in the `DataAccess` folder has a single responsibility, which is to interact with the database by providing methods for retrieving, adding, updating, and deleting employees. It doesn't have any responsibilities beyond this.

- The `SalaryCalculator` class in the `UseCases` folder has a single responsibility, which is to calculate the salary of an employee based on the provided rules.

- The `Menu` class in the `Application` folder is responsible for handling the user interface.

#### Open-Closed Principle

The Open-Closed Principle states that a class should be open for extension but closed for modification. In my project:

- The `Employee` class in the `Domain` folder is open for extension as it allows for the creation of derived classes like Developer, Manager, and HRManager without modifying the base class.

- The `EmployeeFactory` class in the `Domain` folder follows the Open-Closed Principle by allowing the addition of new methods for creating new types of employees without modifying the existing code.

- The `Menu` class in the Appli`cation folder allows for adding new menu options without modifying the class itself. It uses the builder pattern to create the menu options:

```
OptionsHandler<int> menuOptionsHandler = new OptionsHandler<int>()
    .AddOption(1, ListEmployees)
    .AddOption(2, AddEmployee)
    .AddOption(3, UpdateEmployee)
    .AddOption(4, DeleteEmployee)
    .AddOption(5, () => exitMenu = true)
    .SetDefaultFallback(() => Console.WriteLine("Invalid choice."));
```

#### Liskov Substitution Principle

The Liskov Substitution Principle states that objects of derived classes should be able to replace objects of the base class without affecting program correctness. In my project:

- The derived classes Developer, Manager, and HRManager extend the base class Employee and can be used interchangeably wherever an Employee object is expected. For example, the `EmployeeRepository` class in the `DataAccess` folder can work with any type of employee:

```
// The function can take as a parameter any type of employee

public Employee AddEmployee(Employee employee)
{
    List<Employee> employees = GetAllEmployees();
    employees.Add(employee);
    SaveEmployeesToJson(employees);
    return employee;
}
```

#### Interface Segregation Principle

The Interface Segregation Principle states that clients should not be forced to depend on interfaces they do not use. In my project:

- The `IEmployeeFactory` and `IEmployeeRepository` interfaces in the Domain.Interfaces folder are focused and provide only the methods required for their respective responsibilities. Clients can implement these interfaces without being burdened by unnecessary methods. All the methods that are present in the interfaces are also implemented in the respective classes.

#### Dependency Inversion Principle

The Dependency Inversion Principle states that high-level modules should not depend on low-level modules. Both should depend on abstractions, and abstractions should not depend on details; details should depend on abstractions. In my project:

- The `EmployeeRepository` class in the `DataAccess` folder depends on the `IEmployeeRepository` interface in the `Domain.Interfaces` folder. This allows for the `EmployeeRepository` class to be easily replaced with another class that implements the `IEmployeeRepository` interface. The same principle applies to the `EmployeeFactory` class in the `Domain` folder.

In the project, I applied the Repository architectural pattern, which separated the data access logic (interaction with the database) from the business logic. I created an interface for the repository, and a concrete class that implements the interface. The `Menu` class is using dependency injection to get an instance of the `EmployeeRepository` class (and of the `EmployeeFactory` class):

```
public Menu()
    {
        _employeeFactory = new EmployeeFactory();
        _employeeRepository = new EmployeeRepository();

        // ...

    }
```


## Conclusion

Overall, the project showcases well-structured and maintainable code that aligns with SOLID principles and employs design patterns to improve the architecture of the application. This foundation not only facilitates the current functionality of the employee management system but also provides a solid base for future enhancements and modifications.