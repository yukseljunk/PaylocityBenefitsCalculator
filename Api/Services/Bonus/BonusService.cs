using Api.Calculation.CalculationRules;
using Api.Calculation.RuleEngine;
using ErrorOr;
using Api.Extensions;

namespace Api.Services;

public class BonusService : IBonusService
{
    private ICalculationRuleEngine _calculationRuleEngine { get; }

    public BonusService(ICalculationRuleEngine calculationRuleEngine)
    {
        _calculationRuleEngine = calculationRuleEngine;
    }


    public async Task<ErrorOr<Dictionary<ICalculationRule, decimal>>> CalculateBonus(Models.Employee employee, int weekNumber)
    {
        var firstDayForWeekNo = DateTime.Today.FirstDateOfWeek(weekNumber * 2 - 1);//reference date is the first day of the biweek period
        return _calculationRuleEngine.Calculate(employee, firstDayForWeekNo);
    }
}
