namespace TMPS.Domain.Models
{
    public class Manager : Employee
    {
        public int TeamSize { get; set; }

        public Manager(string? name, string? position, decimal baseSalary, int teamSize) : base(name, position, baseSalary)
        {
            TeamSize = teamSize;
        }

        public override decimal CalculateSalary()
        {
            decimal bonus = 0.15M * BaseSalary;
            return BaseSalary + bonus;
        }
    }
}