using Api.Models;
namespace Api.Calculation.CalculationRules;

public abstract class CalculationRuleBase : ICalculationRule
{
    public virtual decimal Effect(IEmployee employee, DateTime referenceDate)
    {
        return 0;
    }

    public virtual bool Eligible(IEmployee employee, DateTime referenceDate)
    {
        return false;
    }
}


