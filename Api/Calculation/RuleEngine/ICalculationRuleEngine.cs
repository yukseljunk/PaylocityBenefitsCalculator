using Api.Calculation.CalculationRules;
using Api.Models;

namespace Api.Calculation.RuleEngine;

public interface ICalculationRuleEngine
{
    Dictionary<ICalculationRule, decimal> Calculate(IEmployee employee, DateTime referenceDate);

}


