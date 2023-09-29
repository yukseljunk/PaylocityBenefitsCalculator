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
            new BiweeklySalaryCalculationRule(),
            new BaseSalaryCalculationRule(),
            new DependentCalculationRule(),
            new KidsCalculationRule(),
            new DependentsOverAgeRule(),
            new YearlySalaryExceedRule()
        };
    }

    public Dictionary<ICalculationRule, decimal> Calculate(IEmployee employee,
        int weekNo, 
        Dictionary<int, Tuple<DateTime, DateTime>> weeksForYear, 
        Dictionary<int, int> paycheckCountForMonth)
    {

        if(!weeksForYear.ContainsKey(weekNo))
        {
            return null;
        }

        var result = new Dictionary<ICalculationRule, decimal>();
        employee.BiWeeklySalary = 0;
        foreach (var rule in calculationRules)
        {
            rule.WeeksForYear= weeksForYear;
            rule.PaycheckCountForMonth= paycheckCountForMonth;

            if (rule.Eligible(employee, weekNo))
            {
                var effect = rule.Effect(employee, weekNo);
                employee.BiWeeklySalary += effect;
                result.Add(rule, Decimal.Round(effect,2));
            }
        }
        employee.BiWeeklySalary = Decimal.Round(employee.BiWeeklySalary, 2);
        return result;
    }
}


