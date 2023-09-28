using Api.Calculation.CalculationRules;
using Api.Models;

namespace Api.Calculation.RuleEngine;

public interface ICalculationRuleEngine
{
    Dictionary<ICalculationRule, decimal> Calculate(IEmployee employee, int weekNo, Dictionary<int, Tuple<DateTime, DateTime>> weeksForYear, Dictionary<int, int> paycheckCountForMonth);

}


