using System.Collections;
using TMPS.Application;
using TMPS.DataAccess.Departments;
using TMPS.DataAccess.Employees;
using TMPS.Domain.Factory;
using TMPS.Domain.Interfaces;


// IEmployeeFactory employeeFactory = new EmployeeFactory();
// IEmployeeRepository employeeRepository = EmployeeRepository.GetInstance();
// IDepartmentRepository departmentRepository = DepartmentRepository.GetInstance();

// EmployeeRepositoryDecorator cachedEmployeeRepository = new EmployeeRepositoryCacheDecorator(employeeRepository);

// Menu menu = new(employeeFactory, cachedEmployeeRepository, departmentRepository);
// menu.Show();



var instance = new MyEnumerable();

foreach (var item in instance)
{
    Console.WriteLine(item);
}

class MyEnumerable : IEnumerable<int>, IEnumerator<int>
{
    public int Current { get; private set; } = 0;

    object IEnumerator.Current => Current;

    public void Dispose() => Reset();

    public IEnumerator<int> GetEnumerator()
    {
        return this;
    }

    public bool MoveNext()
    {
        Current++;
        return Current < 10;
    }

    public void Reset()
    {
        Current = 0;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}