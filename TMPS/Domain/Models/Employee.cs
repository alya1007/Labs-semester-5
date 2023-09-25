namespace TMPS.Domain.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public decimal BaseSalary { get; set; }

        public Employee(int id, string? name, string? position, decimal baseSalary)
        {
            Id = id;
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