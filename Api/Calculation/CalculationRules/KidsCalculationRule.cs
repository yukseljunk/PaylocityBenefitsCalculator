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
            var refDate = child.DateOfBirth;

            var firstOfEndOfWeek = new DateTime(endOfWeek.Year, endOfWeek.Month, 1);
            //if kid is over 18 at the start of the month of the week's end date, it is not counted
            if (child.Age(firstOfEndOfWeek) > MaxAgeForKids)
            {
                continue;
            }
            total += GetBonusWithRespectToDate(weekNo, startOfweek, endOfWeek, refDate, Amount);
            
        }
        return total;
    }

    public override string Name => "Kids bonus";

}


