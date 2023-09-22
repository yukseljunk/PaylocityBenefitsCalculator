using Api.Models;
namespace Api.Calculation.CalculationRules;

public class DependentCalculationRule : CalculationRuleBase
{
    private decimal Amount;
    public DependentCalculationRule(decimal amount = 600)
    {
        Amount = amount;

    }
    public override bool Eligible(IEmployee employee, DateTime referenceDate)
    {
        return employee.Dependents.Any(e => e.Relationship == Relationship.Spouse || e.Relationship == Relationship.DomesticPartner);
    }

    public override decimal Effect(IEmployee employee, DateTime referenceDate)
    {
        return Amount;
    }

}


