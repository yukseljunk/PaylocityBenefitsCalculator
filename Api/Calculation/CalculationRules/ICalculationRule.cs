using Api.Models;

namespace Api.Calculation.CalculationRules;

public interface ICalculationRule
{

    IEmployee Employee { get; set; }
    bool ToBeApplied();

    decimal Effect();


}


