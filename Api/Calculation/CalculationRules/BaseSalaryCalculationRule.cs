namespace Api.Calculation.CalculationRules;

public class BaseSalaryCalculationRule : CalculationRuleBase
{
    private decimal BaseSalary;

    public BaseSalaryCalculationRule(decimal baseSalary = 1000)
    {
        BaseSalary = baseSalary;
    }

    public override bool ToBeApplied()
    {
        return true;
    }


}


