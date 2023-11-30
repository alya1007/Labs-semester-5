namespace TMPS.Domain.Interfaces
{
    public interface IWorkUnit
    {
        string? Name { get; set; }
        decimal CalculateSalary();
    }
}