using Api.Models;
namespace Api.Calculation.CalculationRules;

public class KidsCalculationRule : CalculationRuleBase
{
    private int MaxAgeForKids;
    private decimal Amount;
    public KidsCalculationRule(int maxAgeForKids = 18, decimal amount = 600)
    {

        MaxAgeForKids = maxAgeForKids;
        Amount = amount;
    }

    public override bool Eligible(IEmployee employee, int weekNo)
    {
        return employee.Dependents.Any(e => e.Relationship == Relationship.Child);
    }

    public override decimal Effect(IEmployee employee, int weekNo)
    {
        return employee.Dependents.Count(e => e.Relationship == Relationship.Child) *Amount / 2;
    }

    public override string Name => "Kids bonus";

}


