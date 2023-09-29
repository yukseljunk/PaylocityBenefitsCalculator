using Api.Calculation.CalculationRules;
using System;
using Xunit;

namespace ApiTests.UnitTests
{
    public class YearlySalaryExceedRuleTest : CalculationRulesTest
    {

        [Theory]
        [InlineData(0, true, 60000)]
        [InlineData(0, false, 60000)]
        [InlineData(-200, true, 260000)]
        [InlineData(-200, false, 260000)]
        public void CheckEmployeeSalary(decimal expected, bool forTwicePaid, decimal salary)
        {
            var rule = new YearlySalaryExceedRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };
            var employeeSalary = EmployeeWithoutDependent.Salary;
            EmployeeWithoutDependent.Salary = salary;
            var reflist = forTwicePaid ? WeekNumbersForTwicePaidMonth : WeekNumbersForThricePaidMonth;
            foreach (var weekNo in reflist)
            {
                if (!rule.Eligible(EmployeeWithoutDependent, weekNo)) continue;
                var effect = rule.Effect(EmployeeWithoutDependent, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
            EmployeeWithoutDependent.Salary = employeeSalary;
        }
        
    }
}
