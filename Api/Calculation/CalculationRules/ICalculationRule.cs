using Api.Models;

namespace Api.Calculation.CalculationRules;

public interface ICalculationRule
{
    string Name { get; }
    bool Eligible(IEmployee employee, int weekNo);

    decimal Effect(IEmployee employee, int weekNo);

    Dictionary<int, Tuple<DateTime, DateTime>> WeeksForYear { get; set; }
    Dictionary<int, int> PaycheckCountForMonth { get; set; }

}


