namespace TMPS.Domain.Models
{
    public class HRManager : Employee
    {
        public string Department { get; set; }

        public HRManager(int id, string? name, string? position, decimal baseSalary, string department) : base(id, name, position, baseSalary)
        {
            Department = department;
        }

        public override decimal CalculateSalary()
        {
            decimal bonus = 0.12M * BaseSalary;
            return BaseSalary + bonus;
        }

    }
}