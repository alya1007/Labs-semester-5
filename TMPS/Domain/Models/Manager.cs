namespace TMPS.Domain.Models
{
    public class Manager : Employee
    {
        public int TeamSize { get; set; }

        public Manager(int id, string? name, string? position, decimal baseSalary, int teamSize) : base(id, name, position, baseSalary)
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