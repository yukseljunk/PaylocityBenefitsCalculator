using Api.Models;

namespace Api.Calculation.CalculationRules;

public abstract class CalculationRuleBase : ICalculationRule
{
    public virtual string Name => "";

    public Dictionary<int, Tuple<DateTime, DateTime>> WeeksForYear { get; set; }
    public Dictionary<int, int> PaycheckCountForMonth { get; set; }

    public virtual decimal Effect(IEmployee employee, int weekNo)
    {
        return 0;
    }

    public virtual bool Eligible(IEmployee employee, int weekNo)
    {
        return false;
    }
}


