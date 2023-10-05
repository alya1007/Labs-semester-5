# Topic: Creational Design Patterns

## Author: Alexandra Konjevic

### Group: FAF-213

## Objectives

- Study and understand the Creational Design Patterns.

- Choose a domain, define its main classes/models/entities and choose the appropriate instantiation mechanisms.

- Use some creational design patterns for object instantiation in a sample project.

## Theory

The main concern of the creational design pattern is the way of creating objects. These design patterns are used when a decision must be made at the time of instantiation of a class. Hard-coded way of instantiating an object (with the `new` keyword), isn't always the best way to do it. Sometimes, the nature of the object must be changed according to the nature of the program. In such cases, we must get the help of creational design patterns to provide more general and flexible approach. There are six types of creational design patterns:

- Singleton

- Factory Method

- Abstract Factory

- Builder

- Prototype

- Object Pooling

## Main tasks

- Choose an OO programming language and a suitable IDE or Editor (No frameworks/libs/engines allowed)

- Select a domain area for the sample project

- Define the main involved classes and think about what instantiation mechanisms are needed

- Based on the previous point, implement at least 2 creational design patterns in your project.

## Implementation

### Factory Method

Factory Method is a creational design pattern that provides an interface for creating objects in a superclass, but allows subclasses to alter the type of objects that will be created. The Factory Method pattern suggests that you replace direct object construction calls (using the new operator) with calls to a special factory method. The structure of the Factory Method pattern is the following:

- `Creator` class - declares the factory method that returns new product objects. It's important that the return type of this method matches the product abstraction.

- `Product` abstraction - declares an interface for a type of product object.

- `Concrete Product` - implements the product interface.

- `Concrete Creator` - overrides the factory method to return an instance of a Concrete Product.

In my project, I use the factory pattern for creating `EmployeeFactory`. This factory implements the `IEmployeeFactory`:

```csharp
    public interface IEmployeeFactory
        {
        public Employee CreateDeveloper(string name, decimal baseSalary);
        public Employee CreateManager(string name, decimal baseSalary);
        public Employee CreateHRManager(string name, decimal baseSalary);
        }

```

The interface uses the employee abstraction (`Employee` class) as a return type for the methods, and the `EmployeeFactory` returns concrete instances of employees: `Developer`, `Manager` and `HRManager`.

```csharp
    public Employee CreateDeveloper(string name, decimal baseSalary)
    {
        return new Developer(name, baseSalary, new List<string>());
    }

    public Employee CreateManager(string name, decimal baseSalary)
    {
        return new Manager(name, baseSalary, 0);
    }

    public Employee CreateHRManager(string name, decimal baseSalary)
    {
        return new HRManager(name, baseSalary, "");
    }
```

### Singleton

Singleton is a creational design pattern that lets you ensure that a class has only one instance, while providing a global access point to this instance. The Singleton class declares the static method `GetInstance` that returns the same instance of its own class. The Singleton’s constructor should be hidden from the client code. Calling the `GetInstance` method should be the only way of getting the Singleton object.

In my project, I use the singleton pattern for creating the `EmployeeRepository` class. This class provides methods for adding, removing and getting employees from the database (`employees.json`). There is no need to create multiple instances of this class, so I use the singleton pattern to ensure that only one instance of the class exists.

```csharp
    public class EmployeeRepository
    {
        private static EmployeeRepository? _instance;

        private EmployeeRepository() { }

        public static EmployeeRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EmployeeRepository();
            }
            return _instance;
        }
    }
```

### Builder Pattern

Builder is a creational design pattern that lets you construct complex objects step by step. The pattern allows you to produce different types and representations of an object using the same construction code.

In my project, I used the builder pattern for creating options for the menu, and for this purpose, I used the class `OptionsHandler`. This class has the method `AddOption`, which is used to build the menu using several options.

```csharp
    OptionsHandler<int> menuOptionsHandler = new OptionsHandler<int>()
        .AddOption(1, ListEmployees)
        .AddOption(2, AddEmployee)
        .AddOption(3, UpdateEmployee)
        .AddOption(4, DeleteEmployee)
        .AddOption(5, GetEmployeeReport)
        .AddOption(6, () => exitMenu = true)
        .SetDefaultFallback(() => Console.WriteLine("Invalid choice."));

    menuOptionsHandler.HandleOption(choice);
```

It takes as parameters the option and the action that will be executed when the option is selected. The method returns the `OptionsHandler` object, so that the methods can be chained.

```csharp
    public OptionsHandler<T> AddOption(T option, Action action)
    {
        _optionHandlers.Add(option, action);
        return this;
    }
```

The `SetDefaultFallback` method is used to set the default action that will be executed when the user enters an invalid option.

```csharp
    public OptionsHandler<T> SetDefaultFallback(Action action)
    {
        _defaultFallback = action;
        return this;
    }
```

And the `HandleOption` method is used to execute the action for the selected option.

```csharp
    public void HandleOption(T option)
    {
        if (_optionHandlers.ContainsKey(option))
        {
            _optionHandlers[option]();
        }
        else
        {
            _defaultFallback();
        }
    }
```

### Prototype Pattern

Prototype is a creational design pattern that lets you copy existing objects without making your code dependent on their classes. The structure of the Prototype pattern is the following:

- `Prototype` interface - declares the cloning methods. In most cases, it’s a single `Clone()` method.

- `Concrete Prototype` - implements the cloning method. In addition to copying the original object’s data to the clone, this method may also handle some edge cases of the cloning process related to cloning linked objects, untangling recursive dependencies, etc.

- `Client` - can produce a copy of any object that follows the prototype interface.

In my project, I use the prototype pattern for cloning reports. The prototype interface that I use is `IPrototype`:

```csharp
    public interface IPrototype<T>
    {
        public T Clone();
    }
```

The concrete prototype that I use is `Report` class:

```csharp
    public class Report : IPrototype<Report>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Footer { get; set; }

        public Report(string title, string content, string footer) {}

        public void Print() {}

        public Report Clone() => (Report)MemberwiseClone();
    }

```

I have several prototypes of the reports, which can be cloned using the static class `ReportPrototypes`:

```csharp
    public static class ReportPrototypes
    {
        public static Report DefaultReport(Employee employee) { return new Report(); }

        public static Report DetailedReport(Employee employee) { return new Report(); }

        public static Report ProgressReport(Employee employee) { return new Report(); }
    }
```

The cloning of the prototypes takes place in the `Menu`:

```csharp
    OptionsHandler<int> reportTypesHandler = new OptionsHandler<int>()
        .AddOption(1, () => { Report report = ReportPrototypes.DefaultReport(GetEmployeeForReport()).Clone(); })
        .AddOption(2, () => { Report report = ReportPrototypes.DetailedReport(GetEmployeeForReport()).Clone(); })
        .AddOption(3, () => { Report report = ReportPrototypes.ProgressReport(GetEmployeeForReport()).Clone(); })
        .SetDefaultFallback(() => Console.WriteLine("Invalid choice."));

```

## Conclusion

In this laboratory work, I studied the creational design patterns and implemented some of them in my project. I used the factory method for creating employees, the singleton pattern for creating the employee repository, the builder pattern for creating the menu options, and the prototype pattern for cloning reports. I learned that the creational design patterns are very useful for creating objects in a more flexible way. They allow us to create objects without specifying their concrete classes, and this way, we can change the nature of the objects according to the nature of the program. The creational design patterns also help us to avoid tight coupling between the classes.
