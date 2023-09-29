using Api.Calculation.CalculationRules;
using System;
using Xunit;

namespace ApiTests.UnitTests
{
    public class DependentCalculationRuleTest : CalculationRulesTest
    {

        [Theory]
        [InlineData(0, true, false)]
        [InlineData(0, false, false)]
        [InlineData(300, true, true)]
        [InlineData(200, false, true)]
        public void CheckEmployeeDependent(decimal expected, bool forTwicePaid, bool withDependent)
        {
            var rule = new DependentCalculationRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };
            var employee = withDependent ? EmployeeWithSpouse : EmployeeWithoutDependent;

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
