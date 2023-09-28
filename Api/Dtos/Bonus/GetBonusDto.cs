using Api.Calculation.CalculationRules;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Employee;

public record GetBonusDto
{
    public int WeekNo { get; set; }

    public int EmployeeId { get; set; }

    public decimal AmountToPay { get; set; }

    public Dictionary<string, decimal>? Details { get; set; }


}
