using System.Collections;
using TMPS.Domain.Enumerators;
using TMPS.Domain.Interfaces;
using TMPS.UseCases.Employees;

namespace TMPS.Domain.Models.Abstractions
{
    public class Department : IWorkUnit, IEnumerable<IWorkUnit>, IDepartmentEnumerator
    {
        public string? Name { get; set; }

        public IWorkUnit Current { get; private set; } = null!;

        object IEnumerator.Current => Current;

        public List<IWorkUnit> Teams = new();

        public Department(string name)
        {
            Name = name;
        }

        public void AddTeam(IWorkUnit team)
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

        public bool MoveNext()
        {
            if (Current == null)
            {
                Current = Teams[0];
                return true;
            }

            int index = Teams.IndexOf(Current);
            if (index < Teams.Count - 1)
            {
                Current = Teams[index + 1];
                return true;
            }

            return false;
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
}