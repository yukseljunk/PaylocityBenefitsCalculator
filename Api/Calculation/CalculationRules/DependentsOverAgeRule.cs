using Api.Models;
namespace Api.Calculation.CalculationRules;

public class DependentsOverAgeRule : CalculationRuleBase
{
    private int AgeThreshold;
    private decimal Amount;
    public DependentsOverAgeRule(int ageThreshold = 50, decimal amount = 200)
    {

        AgeThreshold = ageThreshold;
        Amount = amount;
    }

    public override bool Eligible(IEmployee employee, int weekNo)
    {
        var spouse = employee.Dependents.FirstOrDefault(e => e.Relationship == Relationship.Spouse || e.Relationship == Relationship.DomesticPartner);
        return spouse != null;

    }

    public override decimal Effect(IEmployee employee, int weekNo)
    {

        var spouse = employee.Dependents.First(e => e.Relationship == Relationship.Spouse || e.Relationship == Relationship.DomesticPartner);

        var week = WeeksForYear[weekNo];
        var startOfweek = week.Item1;
        var endOfWeek = week.Item2;
        var refDate = spouse.DateOfBirth.AddYears(AgeThreshold);

        var transferFromPreviousMonth = false;
        //find which week child is born if born this year
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
            //add the previous close of month's kid support to this month's biweekly checks
             return 2 * Amount / PaycheckCountForMonth[endOfWeek.Month];
        }

        //if child was born before the start of biweek, then divide amount per the # of paychecks per week's month
        if (refDate < startOfweek)
        {
            return Amount / PaycheckCountForMonth[endOfWeek.Month];
        }

        //if it was born during this biweek, check if born on same month, i.e. not transferred from previous month
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

                return  Amount / paycheckCountLeft;
            }
        }
        return 0;

    }

    public override string Name => "Spouse/Dom. Partner Over 50 Bonus";

}


