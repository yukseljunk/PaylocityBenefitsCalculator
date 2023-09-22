using Api.Calculation.CalculationRules;
using Api.Models;

namespace Api.Calculation.RuleEngine;

public class CalculationRuleEngine : ICalculationRuleEngine
{

    private IList<ICalculationRule> calculationRules;

    /// <summary>
    /// Add calculation rules here in the order of calculation, topologically sorted
    /// </summary>
    public CalculationRuleEngine()
    {
        calculationRules = new List<ICalculationRule>() {

            new BaseSalaryCalculationRule(),
            new DependentCalculationRule(),
            new KidsCalculationRule(),
            new DependentsOverAgeRule(),
            new YearlySalaryExceedRule()

        };

    }

    public Dictionary<ICalculationRule, decimal> Calculate(IEmployee employee)
    {

        var result= new Dictionary<ICalculationRule, decimal>();    
        foreach (var rule in calculationRules)
        {
            rule.Employee = employee;
            if (rule.ToBeApplied())
            {
                result.Add(rule, rule.Effect());
            }
        }

        return result;
    }
}


