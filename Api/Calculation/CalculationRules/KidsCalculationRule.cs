using Api.Models;
namespace Api.Calculation.CalculationRules;

public class KidsCalculationRule : CalculationRuleBase
{
    private int MaxAgeForKids;
    private decimal Amount;
    public KidsCalculationRule(int maxAgeForKids = 18, decimal amount = 600)
    {

        MaxAgeForKids = maxAgeForKids;
        Amount = amount;
    }

    public override bool Eligible(IEmployee employee, int weekNo)
    {
        return employee.Dependents.Any(e => e.Relationship == Relationship.Child);
    }

    public override decimal Effect(IEmployee employee, int weekNo)
    {
        decimal total = 0;
        var children = employee.Dependents.Where(e => e.Relationship == Relationship.Child);
        var week = WeeksForYear[weekNo];
        var startOfweek = week.Item1;
        var endOfWeek = week.Item2;

        foreach (var child in children)
        {
            var firstOfEndOfWeek = new DateTime(endOfWeek.Year, endOfWeek.Month, 1);
            //if kid is over 18 at the start of the month of the week's end date, it is not counted
            if (child.Age(firstOfEndOfWeek) > MaxAgeForKids)
            {
                continue;
            }

            var transferFromPreviousMonth = false;
            //find which week child is born if born this year
            var bornWeek = WeeksForYear.FirstOrDefault(i => child.DateOfBirth >= i.Value.Item1 && child.DateOfBirth <= i.Value.Item2);
            if (bornWeek.Key > 0)
            {
                var endOfBornWeek = bornWeek.Value.Item2;
                if (endOfBornWeek.Month != child.DateOfBirth.Month && endOfWeek.Month == child.DateOfBirth.Month+1)
                {
                    transferFromPreviousMonth = true;
                }
            }
            if (transferFromPreviousMonth)
            {
                //add the previous close of month's kid support to this month's biweekly checks
                total += 2 * Amount / PaycheckCountForMonth[endOfWeek.Month];
                continue;
            }

            //if child was born before the start of biweek, then divide amount per the # of paychecks per week's month
            if (child.DateOfBirth < startOfweek)
            {
                total += Amount / PaycheckCountForMonth[endOfWeek.Month];
                continue;
            }

            //if it was born during this biweek, check if born on same month, i.e. not transferred from previous month
            if (child.DateOfBirth >= startOfweek && child.DateOfBirth <= endOfWeek)
            {
                if (endOfWeek.Month == child.DateOfBirth.Month)
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

                    total += Amount / paycheckCountLeft;
                    continue;
                }
            }
        }
        return total;
    }

    public override string Name => "Kids bonus";

}


