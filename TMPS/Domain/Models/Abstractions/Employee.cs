namespace TMPS.Domain.Models.Abstractions
{
    public abstract class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal BonusCoefficient { get; set; }

        public Employee(int id, string? name, decimal baseSalary)
        {
            Id = id;
            Name = name;
            BaseSalary = baseSalary;
        }
    }
}