# Topic: Structural Design Patterns

## Author: Alexandra Konjevic

### Group: FAF-213

## Objectives

- Study and understand the Structural Design Patterns.

- As a continuation of the previous laboratory work, think about the functionalities that your system will need to provide to the user.

- Implement some additional functionalities, or create a new project using structural design patterns.

## Theory

Structural design patterns explain how to assemble objects and classes into larger structures, while keeping these structures flexible and efficient. The structural patterns are concerned with how classes and objects are composed to form larger structures. The structural design patterns are:

- Adapter

- Bridge

- Composite

- Decorator

- Facade

- Flyweight

- Proxy

## Main tasks

- Implement at least 2 structural design patterns in your project.

- Keep the files grouped by their responsibilities.

- Document the work in a separate markdown file according to the requirements.

## Implementation

### Decorator

Decorator is a structural design pattern that lets you attach new behaviors to objects by placing these objects inside special wrapper objects that contain the behaviors. In my project, I use the Decorator to create a caching mechanism for the `EmployeeRepository`. The classes that form the structure of the Decorator pattern are:

- `Component` - declares the common interface for both wrappers and wrapped objects. In my project, this is the `IEmployeeRepository` interface. From this interface, both the `EmployeeRepository` and the `EmployeeRepositoryDecorator` classes inherit.

- `Concrete Component` - defines the basic behavior, which can be altered by decorators. In my project, this is the `EmployeeRepository` class.

- `Base Decorator` - is a base class for all concrete decorators and contains the reference to the wrapped object. In my project, this is the `EmployeeRepositoryDecorator` class.

- `Concrete Decorator` - adds new behaviors to the wrapped object. In my project, this is the `EmployeeRepositoryCacheDecorator` class:

  - `GetAllEmployees`: When the `GetAllEmployees` method is called for the first time, the decorator checks if the cache is empty `(_cache.Count == 0)`. If the cache is empty, it calls the base `GetAllEmployees` method to retrieve the data from the actual repository. It then iterates through the returned employees and stores them in the cache using their IDs as keys. Subsequent calls to `GetAllEmployees` return the data from the cache, avoiding a trip to the actual repository.

  ```csharp
  public new List<Employee> GetAllEmployees()
      {
          if (_cache.Count == 0)
          {
              var employees = base.GetAllEmployees();
              foreach (var employee in employees)
              {
                  _cache[employee.Id.ToString()] = employee;
              }
          }

          return _cache.Values.ToList();
      }
  ```

  - `GetEmployeeById`: When the `GetEmployeeById` method is called, the decorator first checks if the employee with the specified ID is in the cache. If it is, it returns the employee from the cache. If it is not, it calls the base `GetEmployeeById` method to retrieve the employee from the actual repository. It then stores the employee in the cache using its ID as a key.

  ```csharp
  public new Employee GetEmployeeById(string id)
  {
      if (_cache.ContainsKey(id))
      {
          return _cache[id];
      }

      var employee = base.GetEmployeeById(id);
      _cache[id] = employee;
      return employee;
  }
  ```

  - `AddEmployee`: When the `AddEmployee` method is called, the decorator first calls the base `AddEmployee` method to add the employee to the actual repository. It then stores the employee in the cache using its ID as a key.

  ```csharp
    public new Employee AddEmployee(Employee employee)
    {
        var addedEmployee = base.AddEmployee(employee);
        _cache[addedEmployee.Id.ToString()] = addedEmployee;
        return addedEmployee;
    }
  ```

  - `UpdateEmployee`: When the `UpdateEmployee` method is called, the decorator first calls the base `UpdateEmployee` method to update the employee in the actual repository. It then stores the employee in the cache using its ID as a key.

  ```csharp
  public new Employee UpdateEmployee(Employee employee)
  {
      var updatedEmployee = base.UpdateEmployee(employee);
      _cache[updatedEmployee.Id.ToString()] = updatedEmployee;
      return updatedEmployee;
  }
  ```

  - `DeleteEmployee`: When the `DeleteEmployee` method is called, the decorator first calls the base `DeleteEmployee` method to delete the employee from the actual repository. It then removes the employee from the cache.

  ```csharp
  public new void DeleteEmployee(string id)
  {
      base.DeleteEmployee(id);
      _cache.Remove(id);
  }
  ```

- `Client` - can wrap components in multiple layers of decorators, as long as it works with all objects via the component interface. In my project, this is the `Program` class. It creates an instance of the `EmployeeRepositoryCacheDecorator` class and uses it to retrieve the employees from the repository.

```csharp
IEmployeeRepository employeeRepository = EmployeeRepository.GetInstance();
EmployeeRepositoryDecorator cachedEmployeeRepository = new EmployeeRepositoryCacheDecorator(employeeRepository);
```

### Composite

Compose objects into tree structures to represent part-whole hierarchies. Composite lets clients treat individual objects and compositions of objects uniformly. In my project, I created a hierarchal system of compound teams, that can contain other teams or employees. The classes that form the structure of the Composite pattern are:

- `Component` - The Component interface describes operations that are common to both simple and complex elements of the tree. In my project, this is the `IDepartment` class, which has the method `CalculateSalary`, that can calculate the total salary of a department and of the teams in the department.

```csharp
    public interface IDepartment
    {
        decimal CalculateSalary();
    }
```

- `Leaf` - The Leaf is a basic element of a tree. Usually, leaf components end up doing most of the real work, since they don’t have anyone to delegate the work to. In my project, the leafs is the `Team` class, which implements `CalculateSalary` in the following way:

```csharp
    public decimal CalculateSalary()
    {
        decimal totalSalary = 0;
        foreach (var employee in Employees)
        {
            totalSalary += SalaryCalculator.CalculateSalary(employee);
        }
        return totalSalary;
    }
```

- `Composite` - The Container (aka composite) is an element that has sub-elements: leaves or other containers. A container doesn’t know the concrete classes of its children. It works with all sub-elements only via the component interface. Upon receiving a request, a container delegates the work to its sub-elements, processes intermediate results and then returns the final result to the client. In my project, the composite class is the `Department` class:

```csharp
    public class Department : IDepartment
    {
        public string? Name { get; set; }
        public List<IDepartment> Teams = new();

        public Department(string name)
        {
            Name = name;
        }

        public void AddTeam(IDepartment team)
        {
            Teams.Add(team);
        }

        public void RemoveTeam(string teamName)
        {
            Teams.RemoveAll(team => team.Name == teamName);
        }

        public decimal CalculateSalary()
        {
            decimal totalSalary = 0;
            foreach (var team in Teams)
            {
                totalSalary += team.CalculateSalary();
            }
            return totalSalary;
        }
    }
```

- `Client` - The Client works with all elements through the component interface. As a result, the client can work in the same way with both simple or complex elements of the tree. In my project, I can create departments (that, with the help of `DepartmentRepository` are stored in a database file - `departments.json`), which can have nested `IDepartment` objects - teams:

```csharp
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
```

In the line `department.AddTeam(team);` I access the method `AddTeam` of the `Department` class, which adds the team to the `Teams` list.

Also, I can access `RemoveTeam` method of the `Department` class, which removes the team from the `Teams` list.

```csharp
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
```

The `CalculateSalary` method which calculated the total salary of a department and of the teams in the department is accessed in the following way:

```csharp
    private void CalculateSalaryOfDepartment()
    {
        System.Console.WriteLine("Enter department name: ");
        string departmentName = Console.ReadLine() ?? "";
        Department department = _departmentRepository.GetDepartmentByName(departmentName);

        System.Console.WriteLine("Total salary of department: " + department.CalculateSalary());
    }
```

### Facade

Facade is a structural design pattern that provides a simplified interface to a library, a framework, or any other complex set of classes. In my project, I use the Facade to create a simplified interface for the `EmployeeRepository` and `DepartmentRepository`. The classes that form the structure of the Facade pattern are:

- `Facade` - provides convenient access to a particular part of the subsystem’s functionality. It knows where to direct the client’s request and how to operate all the moving parts. In my project, these are the interfaces: `IEmployeeRepository` and `IDepartmentRepository`:

```csharp
    public interface IEmployeeRepository
    {
        List<Employee> GetAllEmployees();
        Employee GetEmployeeById(string id);
        Employee AddEmployee(Employee employee);
        Employee UpdateEmployee(Employee employee);
        void DeleteEmployee(string id);
    }
```

```csharp
    public interface IDepartmentRepository
    {
        List<Department> GetDepartments();
        Department GetDepartmentByName(string departmentName);
        void AddDepartment(Department department);
        void UpdateDepartment(Department department);
        void RemoveDepartment(Department department);
    }
```

- `Complex Subsystem` - Subsystem classes aren’t aware of the facade’s existence. They operate within the system and work with each other directly. In my project, the subsystem consists of the `EmployeeRepository` and `DepartmentRepository` classes, which implement all the methods from the interfaces above, for performing the CRUD operations on the employees and departments.

- `Client` - The Client uses the facade instead of calling the subsystem objects directly. In my project, the client (`Menu` class), receives from the entry point of the project - the class `Program` - an instance of the `IEmployeeRepository` and `IDepartmentRepository`:

```csharp
    IEmployeeRepository employeeRepository = EmployeeRepository.GetInstance();
    IDepartmentRepository departmentRepository = DepartmentRepository.GetInstance();
    Menu menu = new(employeeFactory, cachedEmployeeRepository, departmentRepository);
    menu.Show();
```

### Bridge

Bridge is a structural design pattern that lets you split a large class or a set of closely related classes into two separate hierarchies—abstraction and implementation—which can be developed independently of each other.

In my project, I use the Bridge to separate the `EmployeeRepository` and `DepartmentRepository` classes into two hierarchies: abstraction and implementation. The classes that form the structure of the Bridge pattern are:

- `Abstraction` - The Abstraction provides high-level control logic. It relies on the implementation object to do the actual low-level work. In my project, this is the `IEmployeeRepository` interface, which has the methods for performing the CRUD operations on the employees, and the `IDepartmentRepository` interface, which has the methods for performing the CRUD operations on the departments. In the whole project, I use the interfaces to access the methods of the `EmployeeRepository` and `DepartmentRepository` classes, and never the classes themselves.

- `Implementation` - The implementation implements the low-level infrastructure details. An abstraction can only communicate with an implementation object via methods that are declared there. In my project, this is the `EmployeeRepository` and `DepartmentRepository` classes, which implement the methods from the interfaces above.

## Conclusion

In this laboratory work, I studied and understood the Structural Design Patterns. I implemented 4 structural design patterns in my project: Decorator, Composite, Facade and Bridge. I learned how to use the Decorator to create a caching mechanism for the `EmployeeRepository`, how to use the Composite to create a hierarchal system of compound departments, that can contain other teams or employees, how to use the Facade to create a simplified interface for the `EmployeeRepository` and `DepartmentRepository` and how to use the Bridge to separate the `EmployeeRepository` and `DepartmentRepository` classes into two hierarchies: abstraction and implementation.
