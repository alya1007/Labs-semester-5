namespace TMPS.Domain.Interfaces
{
    public interface IDepartment
    {
        string? Name { get; set; }
        decimal CalculateSalary();
    }
}