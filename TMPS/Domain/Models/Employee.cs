namespace TMPS.Domain.Models
{
    public class Employee
    {
        public string? Name { get; set; }
        public string? Position { get; set; }
        public decimal BaseSalary { get; set; }

        public Employee(string? name, string? position, decimal baseSalary)
        {
            Name = name;
            Position = position;
            BaseSalary = baseSalary;
        }

        public virtual decimal CalculateSalary()
        {
            return BaseSalary;
        }
    }
}