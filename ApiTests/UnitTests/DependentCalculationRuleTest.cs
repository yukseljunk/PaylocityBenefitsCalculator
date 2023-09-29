using Api.Calculation.CalculationRules;
using System;
using Xunit;

namespace ApiTests.UnitTests
{
    public class DependentCalculationRuleTest : CalculationRulesTest
    {

        [Theory]
        [InlineData(0, true)]
        [InlineData(0, false)]
        public void CheckEmployeeWithoutDependent(decimal expected, bool forTwicePaid)
        {
            var rule = new DependentCalculationRule
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

        [Theory]
        [InlineData(300, true)]
        [InlineData(200, false)]
        public void CheckEmployeeWithDependent(decimal expected, bool forTwicePaid)
        {
            var rule = new DependentCalculationRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };
            var reflist = forTwicePaid ? WeekNumbersForTwicePaidMonth : WeekNumbersForThricePaidMonth;
            foreach (var weekNo in reflist)
            {
                if (!rule.Eligible(EmployeeWithSpouse, weekNo)) continue;

                var effect = rule.Effect(EmployeeWithSpouse, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
        }
    }
}
