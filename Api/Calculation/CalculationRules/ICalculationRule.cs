using Api.Models;

namespace Api.Calculation.CalculationRules;

public interface ICalculationRule
{
    bool Eligible(IEmployee employee);

    decimal Effect(IEmployee employee);

}


