namespace Api.Calculation.CalculationRules;

public class YearlySalaryExceedRule : CalculationRuleBase
{
    private int SalaryThreshold;
    public YearlySalaryExceedRule(int salaryThreshold = 80000)
    {
        SalaryThreshold = salaryThreshold;
    }
}


