﻿using Api.Models;
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

    public override bool Eligible(IEmployee employee, DateTime referenceDate)
    {
        var spouse = employee.Dependents.FirstOrDefault(e => e.Relationship == Relationship.Spouse || e.Relationship == Relationship.DomesticPartner);
        return spouse == null ? false : spouse.Age(referenceDate) >= AgeThreshold;

    }

    public override decimal Effect(IEmployee employee, DateTime referenceDate)
    {
        return Amount;
    }

}


