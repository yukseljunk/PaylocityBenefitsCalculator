using Api.Models;
namespace Api.Calculation.CalculationRules;

public class BaseSalaryCalculationRule : CalculationRuleBase
{
    private decimal BaseSalary;

    public BaseSalaryCalculationRule(decimal baseSalary = 1000)
    {
        BaseSalary = baseSalary;
    }
    
    public override bool Eligible(IEmployee employee, int weekNo)
    {
        return true;
    }

    public override decimal Effect(IEmployee employee, int weekNo)
    {
        return BaseSalary/PaycheckCountForMonth[WeeksForYear[weekNo].Item2.Month];
    }

    public override string Name => "Base Bonus";

}


