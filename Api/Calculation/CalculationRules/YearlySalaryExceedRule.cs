using Api.Models;
namespace Api.Calculation.CalculationRules;

public class YearlySalaryExceedRule : CalculationRuleBase
{
    private int SalaryThreshold;
    private int Percentage;
    public YearlySalaryExceedRule(int salaryThreshold = 80000, int percentage = 2)
    {
        SalaryThreshold = salaryThreshold;
        Percentage = percentage;
    }
    public override bool Eligible(IEmployee employee, DateTime referenceDate)
    {
        return employee.Salary >= SalaryThreshold;
    }
    public override decimal Effect(IEmployee employee, DateTime referenceDate)
    {
        return -1 * employee.Salary * Percentage / 100;
    }

    public override string Name => $"Yearly Salary Exceed {SalaryThreshold}$";
}


