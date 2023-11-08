using TMPS.Domain.Models.Abstractions;

namespace TMPS.Domain.Models
{
    public class Department : EmployeeComposite
    {
        public string Name { get; set; }
        public List<string> EmployeesIds { get; set; } = new();

        public Department(string name)
        {
            Name = name;
        }
    }
}
