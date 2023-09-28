using Api.Models;
namespace Api.Calculation.CalculationRules;

public class MonthlySalaryCalculationRule : CalculationRuleBase
{
    private decimal BaseSalary;

    public override bool Eligible(IEmployee employee, DateTime referenceDate)
    {
        return true;
    }

    public override decimal Effect(IEmployee employee, DateTime referenceDate)
    {
        return employee.Salary/12;
    }

    public override string Name => "Base Salary";

}


