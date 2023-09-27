using System.Text.Json.Serialization;

namespace TMPS.Domain.Models.Abstractions
{
    [JsonDerivedType(typeof(Developer), typeDiscriminator: "developer")]
    [JsonDerivedType(typeof(Manager), typeDiscriminator: "manager")]
    [JsonDerivedType(typeof(HRManager), typeDiscriminator: "hrmanager")]
    public class Employee
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal BonusCoefficient { get; set; }

    }
}