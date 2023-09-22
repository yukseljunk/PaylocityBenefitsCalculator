namespace Api.Calculation.CalculationRules;

public class KidsCalculationRule : CalculationRuleBase
{
    private int MaxAgeForKids;
    public KidsCalculationRule(int maxAgeForKids = 18)
    {

        MaxAgeForKids = maxAgeForKids;

    }
}


