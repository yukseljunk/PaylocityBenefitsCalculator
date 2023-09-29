using Api.Calculation.CalculationRules;
using Api.ServiceErrors;
using ErrorOr;

namespace Api.Models;

public class Bonus
{
    public int WeekNo { get; set; }

    public Employee? Employee { get; set; }

    public decimal AmountToPay { get; set; }

    public Dictionary<ICalculationRule, decimal>? Details { get; set; }

    private Bonus(int weekNo, Employee? employee, decimal amountToPay, Dictionary<ICalculationRule, decimal>? details)
    {
        WeekNo = weekNo;
        Employee = employee;
        AmountToPay = amountToPay;
        Details = details;
    }

    public static ErrorOr<Bonus> Create(int weekNo, Employee? employee, decimal amountToPay, Dictionary<ICalculationRule, decimal>? details)
    {
        //validations here
        List<Error> errors = new();

        if(weekNo<1 || weekNo > 26)
        {
            errors.Add(BonusErrors.InvalidWeek(weekNo));
        }

        if (errors.Any())
        {
            return errors;
        }

        return new Bonus(weekNo, employee, amountToPay, details);   
    }
}


