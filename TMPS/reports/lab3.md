# Topic: Structural Design Patterns

## Author: Alexandra Konjevic

### Group: FAF-213

## Objectives

- Study and understand the Structural Design Patterns.

- As a continuation of the previous laboratory work, think about the functionalities that your system will need to provide to the user .

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

## Conclusion

In this laboratory work, I studied the creational design patterns and implemented some of them in my project. I used the factory method for creating employees, the singleton pattern for creating the employee repository, the builder pattern for creating the menu options, and the prototype pattern for cloning reports. I learned that the creational design patterns are very useful for creating objects in a more flexible way. They allow us to create objects without specifying their concrete classes, and this way, we can change the nature of the objects according to the nature of the program. The creational design patterns also help us to avoid tight coupling between the classes.
