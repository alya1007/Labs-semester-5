namespace TMPS.Domain.Models
{
    public class Developer : Employee
    {
        public List<string?> Skills { get; set; }

        public Developer(int id, string? name, string? position, decimal baseSalary, List<string?> skills) : base(id, name, position, baseSalary)
        {
            Skills = skills;
        }

        public override decimal CalculateSalary()
        {
            decimal bonus = 0.2M * BaseSalary;
            return BaseSalary + bonus;
        }
    }
}