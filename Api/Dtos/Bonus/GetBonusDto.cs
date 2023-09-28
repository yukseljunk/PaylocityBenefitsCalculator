using Api.Calculation.CalculationRules;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Employee;

public record GetBonusDto
{
    [Range(1, 26)]
    public int WeekNo { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Employee Id cannot be negative.")]
    public int EmployeeId { get; set; }

    public decimal AmountToPay { get; set; }

    public Dictionary<string, decimal>? Details { get; set; }


}
