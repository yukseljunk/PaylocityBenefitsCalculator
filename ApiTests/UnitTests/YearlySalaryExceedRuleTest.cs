using Api.Calculation.CalculationRules;
using System;
using Xunit;

namespace ApiTests.UnitTests
{
    public class YearlySalaryExceedRuleTest : CalculationRulesTest
    {

        [Theory]
        [InlineData(0, true)]
        [InlineData(0, false)]
        public void CheckEmployeeLowSalary(decimal expected, bool forTwicePaid)
        {
            var rule = new YearlySalaryExceedRule
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
        [InlineData(-200, true)]
        [InlineData(-200, false)]
        public void CheckEmployeeHiSalary(decimal expected, bool forTwicePaid)
        {
            var rule = new YearlySalaryExceedRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };
            var salary = EmployeeWithoutDependent.Salary;
            EmployeeWithoutDependent.Salary = 260000m;
            var reflist = forTwicePaid ? WeekNumbersForTwicePaidMonth : WeekNumbersForThricePaidMonth;
            foreach (var weekNo in reflist)
            {
                if (!rule.Eligible(EmployeeWithoutDependent, weekNo)) continue;
                var effect = rule.Effect(EmployeeWithoutDependent, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
            EmployeeWithoutDependent.Salary = salary;

        }

    }
}
