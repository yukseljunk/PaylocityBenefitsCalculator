using Api.Models;

namespace Api.Calculation.CalculationRules;

public interface ICalculationRule
{
    bool Eligible(IEmployee employee, DateTime referenceDate);

    decimal Effect(IEmployee employee, DateTime referenceDate);

}


