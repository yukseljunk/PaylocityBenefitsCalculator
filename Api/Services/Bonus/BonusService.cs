using Api.Calculation.RuleEngine;
using ErrorOr;
using Api.Extensions;
using Api.Models;

namespace Api.Services;

public class BonusService : IBonusService
{
    private ICalculationRuleEngine _calculationRuleEngine { get; }

    public BonusService(ICalculationRuleEngine calculationRuleEngine)
    {
        _calculationRuleEngine = calculationRuleEngine;
    }


    public async Task<ErrorOr<Bonus>> CalculateBonus(Models.Employee employee, int weekNumber)
    {
        var wpCalc = CalculateWeeksAndPaychecks(DateTime.Today);

        var details = _calculationRuleEngine.Calculate(employee, weekNumber, wpCalc.Item1, wpCalc.Item2);
        return Bonus.Create(weekNumber, employee, 0, details);
    }

    public static Tuple<Dictionary<int, Tuple<DateTime, DateTime>>, Dictionary<int, int>> CalculateWeeksAndPaychecks(DateTime refDate)
    {
        var weeksForYear = new Dictionary<int, Tuple<DateTime, DateTime>>();
        var paycheckCountForMonth = new Dictionary<int, int>();
        var firstDayForFirstWeek = refDate.FirstDateOfWeek(1);
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

        return new Tuple<Dictionary<int, Tuple<DateTime, DateTime>>, Dictionary<int, int>>(weeksForYear, paycheckCountForMonth);
    }
}
