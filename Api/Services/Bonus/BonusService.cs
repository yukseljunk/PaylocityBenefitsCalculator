using Api.Calculation.CalculationRules;
using Api.Calculation.RuleEngine;
using ErrorOr;
using Api.Extensions;
using Microsoft.VisualBasic;

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

        var weeksForYear = new Dictionary<int, Tuple<DateTime, DateTime>>();
        var paycheckCountForMonth = new Dictionary<int, int>();
        var firstDayForFirstWeek = DateTime.Today.FirstDateOfWeek(1);
        for (int i = 0; i < 26; i++)
        {
            var startDate = firstDayForFirstWeek.AddDays(i * 14);
            var endDate = startDate.AddDays(13);
            weeksForYear.Add(i + 1, new Tuple<DateTime, DateTime>(startDate, endDate));
            if (paycheckCountForMonth.ContainsKey(endDate.Month))
            {
                paycheckCountForMonth[endDate.Month]++;
            }
            else
            {
                paycheckCountForMonth.Add(endDate.Month, 1);
            }
        }

        return _calculationRuleEngine.Calculate(employee, weekNumber, weeksForYear, paycheckCountForMonth);
    }
}
