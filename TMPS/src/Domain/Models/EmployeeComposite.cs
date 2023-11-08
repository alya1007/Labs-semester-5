using TMPS.Domain.Interfaces;
using TMPS.UseCases.Employees;

namespace TMPS.Domain.Models.Abstractions
{
    public class EmployeeComposite : IEmployeeComponent
    {
        private List<IEmployeeComponent> components = new();

        public void AddComponent(IEmployeeComponent component)
        {
            components.Add(component);
        }

        public void RemoveComponent(IEmployeeComponent component)
        {
            components.Remove(component);
        }

        public decimal CalculateSalary()
        {
            decimal totalSalary = 0;
            foreach (var component in components)
            {
                totalSalary += component.CalculateSalary();
            }
            return totalSalary;
        }
    }
}
