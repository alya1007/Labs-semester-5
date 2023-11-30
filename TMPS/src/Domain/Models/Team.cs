using System.Collections;
using System.Text.Json.Serialization;
using TMPS.DataAccess.Employees;
using TMPS.Domain.Enumerators;
using TMPS.Domain.Interfaces;
using TMPS.Domain.Models.Abstractions;
using TMPS.UseCases.Employees;

namespace TMPS.Domain.Models
{
    public class Team : IWorkUnit, IEnumerable<IWorkUnit>, ITeamEnumerator
    {
        public string? Name { get; set; }
        public List<IWorkUnit> Employees { get; set; } = new();

        public IWorkUnit Current { get; private set; } = null!;

        object IEnumerator.Current => Current;

        public Team(string name)
        {
            Name = name;
        }

        public decimal CalculateSalary()
        {
            decimal totalSalary = 0;
            foreach (var employee in Employees)
            {
                totalSalary += employee.CalculateSalary();
            }
            return totalSalary;
        }

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
            if (Current == null)
            {
                Current = Employees[0];
                return true;
            }

            int index = Employees.IndexOf(Current);
            if (index < Employees.Count - 1)
            {
                Current = Employees[index + 1];
                return true;
            }

            return false;
        }

        public void Reset()
        {
            Current = null!;
        }

        public void Dispose() => Reset();
    }
}