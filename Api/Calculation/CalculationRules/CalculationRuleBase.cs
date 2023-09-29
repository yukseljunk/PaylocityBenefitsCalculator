using Api.Models;

namespace Api.Calculation.CalculationRules;

public abstract class CalculationRuleBase : ICalculationRule
{
    public virtual string Name => "";

    public Dictionary<int, Tuple<DateTime, DateTime>> WeeksForYear { get; set; }
    public Dictionary<int, int> PaycheckCountForMonth { get; set; }

    public virtual decimal Effect(IEmployee employee, int weekNo)
    {
        return 0;
    }

    public virtual bool Eligible(IEmployee employee, int weekNo)
    {
        return false;
    }

    protected decimal GetBonusWithRespectToDate(int weekNo, DateTime startOfweek, DateTime endOfWeek, DateTime refDate, decimal amount)
    {
        var transferFromPreviousMonth = false;
        //find which week refdate is if it is in this year
        var bornWeek = WeeksForYear.FirstOrDefault(i => refDate >= i.Value.Item1 && refDate <= i.Value.Item2);
        if (bornWeek.Key > 0)
        {
            var endOfBornWeek = bornWeek.Value.Item2;
            if (endOfBornWeek.Month != refDate.Month && endOfWeek.Month == refDate.Month + 1)
            {
                transferFromPreviousMonth = true;
            }
        }
        if (transferFromPreviousMonth)
        {
            //add the previous close of month's bonus to this month's biweekly checks
            return 2 * amount / PaycheckCountForMonth[endOfWeek.Month];
        }

        //if refdate is before the start of biweek, then divide amount per the # of paychecks per week's month
        if (refDate < startOfweek)
        {
            return amount / PaycheckCountForMonth[endOfWeek.Month];
        }

        //if refdate is during this biweek, check if it is on same month, i.e. not transferred from previous month
        if (refDate >= startOfweek && refDate <= endOfWeek)
        {
            if (endOfWeek.Month == refDate.Month)
            {
                //find how many more paychecks to be given during this month
                var paycheckCountLeft = 0;

                var currentMonthWeeks = WeeksForYear.Where(i => i.Value.Item2.Month == endOfWeek.Month);
                foreach (var monthWeek in currentMonthWeeks)
                {
                    if (monthWeek.Key >= weekNo)
                    {
                        paycheckCountLeft++;
                    }
                }

                return amount / paycheckCountLeft;
            }
        }
        return 0;
    }
}


