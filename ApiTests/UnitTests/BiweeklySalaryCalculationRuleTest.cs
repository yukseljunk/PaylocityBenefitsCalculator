using Api.Calculation.CalculationRules;
using System;
using System.Data;
using Xunit;

namespace ApiTests.UnitTests
{
    public class BiweeklySalaryCalculationRuleTest : CalculationRulesTest
    {

        [Theory]
        [InlineData(1000, true)]
        [InlineData(1000, false)]
        public void CheckEmployeeWithoutDependent(decimal expected, bool forTwicePaid)
        {
            var rule = new BiweeklySalaryCalculationRule
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
