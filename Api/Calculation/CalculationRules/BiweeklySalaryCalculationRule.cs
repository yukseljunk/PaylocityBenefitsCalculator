using Api.Models;
namespace Api.Calculation.CalculationRules;

public class BiweeklySalaryCalculationRule : CalculationRuleBase
{
    private decimal BaseSalary;

    public override bool Eligible(IEmployee employee, int weekNo)
    {
        return true;
    }

    public override decimal Effect(IEmployee employee, int weekNo)
    {
        return employee.Salary/26;
    }

    public override string Name => "Base Salary";

}


