using Api.Calculation.CalculationRules;
using System;
using Xunit;

namespace ApiTests.UnitTests
{
    public class BaseSalaryCalculationRuleTest: CalculationRulesTest
    {

        [Theory]
        [InlineData(500, true)]
        [InlineData(333.33, false)]
        public void CheckEmployeeWithoutDependent(decimal expected, bool forTwicePaid)
        {
            var rule = new BaseSalaryCalculationRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };
            var reflist = forTwicePaid ? WeekNumbersForTwicePaidMonth : WeekNumbersForThricePaidMonth;
            foreach (var weekNo in reflist)
            {
                if (!rule.Eligible(EmployeeWithoutDependent, weekNo)) continue;
                var effect = rule.Effect(EmployeeWithoutDependent, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
        }

    }
}
