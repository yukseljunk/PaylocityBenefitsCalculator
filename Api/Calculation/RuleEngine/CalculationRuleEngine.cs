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

    public Dictionary<ICalculationRule, decimal> Calculate(IEmployee employee, DateTime referenceDate)
    {

        var result = new Dictionary<ICalculationRule, decimal>();
        employee.MonthlySalary = employee.Salary / 12;
        foreach (var rule in calculationRules)
        {
            if (rule.Eligible(employee, referenceDate))
            {
                var effect = rule.Effect(employee, referenceDate);
                employee.MonthlySalary += effect;
                result.Add(rule, effect);
            }
        }
        return result;
    }
}


