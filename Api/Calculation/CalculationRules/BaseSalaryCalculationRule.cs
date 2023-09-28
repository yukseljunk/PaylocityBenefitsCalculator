using Api.Models;
namespace Api.Calculation.CalculationRules;

public class BaseSalaryCalculationRule : CalculationRuleBase
{
    private decimal BaseSalary;

    public BaseSalaryCalculationRule(decimal baseSalary = 1000)
    {
        BaseSalary = baseSalary;
    }
    
    public override bool Eligible(IEmployee employee, DateTime referenceDate)
    {
        return true;
    }

    public override decimal Effect(IEmployee employee, DateTime referenceDate)
    {
        return BaseSalary/2;
    }

    public override string Name => "Base Bonus";

}


