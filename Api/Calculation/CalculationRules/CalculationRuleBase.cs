using Api.Models;

namespace Api.Calculation.CalculationRules;

public abstract class CalculationRuleBase : ICalculationRule
{
    public virtual decimal Effect(IEmployee employee)
    {
        return 0;
    }

    public virtual bool Eligible(IEmployee employee)
    {
        return false;
    }
}


