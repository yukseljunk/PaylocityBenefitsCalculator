using Api.Models;
namespace Api.Calculation.CalculationRules;

public class DependentCalculationRule : CalculationRuleBase
{
    private decimal Amount;
    public DependentCalculationRule(decimal amount = 600)
    {
        Amount = amount;

    }
    public override bool Eligible(IEmployee employee, int weekNo)
    {
        return employee.Dependents.Any(e => e.Relationship == Relationship.Spouse || e.Relationship == Relationship.DomesticPartner);
    }

    public override decimal Effect(IEmployee employee, int weekNo)
    {
        return Amount/2;
    }

    public override string Name => "Spouse/Domestic Partner Bonus";

}


