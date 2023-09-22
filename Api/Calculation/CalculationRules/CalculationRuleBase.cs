using Api.Models;

namespace Api.Calculation.CalculationRules;

public abstract class CalculationRuleBase : ICalculationRule
{

    public IEmployee Employee { get; set; }

    public decimal Effect()
    {
        return 0;
    }

    public virtual bool ToBeApplied()
    {
        return false;
    }
}


