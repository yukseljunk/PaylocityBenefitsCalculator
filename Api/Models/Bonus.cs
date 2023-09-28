using Api.Calculation.CalculationRules;

namespace Api.Models;

public class Bonus
{
    public int WeekNo { get; set; }

    public Employee? Employee { get; set; }  

    public decimal AmountToPay { get; set; }

    public Dictionary<ICalculationRule, decimal>? Details { get; set; }

}


