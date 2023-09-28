using Api.Calculation.CalculationRules;
using ErrorOr;

namespace Api.Services;

public interface IBonusService
{
    Task<ErrorOr<Dictionary<ICalculationRule, decimal>>> CalculateBonus(Models.Employee employee, int weekNumber);

}
