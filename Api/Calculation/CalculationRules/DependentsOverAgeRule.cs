namespace Api.Calculation.CalculationRules;

public class DependentsOverAgeRule : CalculationRuleBase
{
    private int AgeThreshold;
    public DependentsOverAgeRule(int ageThreshold = 50)
    {

        AgeThreshold = ageThreshold;
    }
}


