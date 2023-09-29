using Api.Calculation.CalculationRules;
using Api.Models;
using System;
using System.Linq;
using Xunit;

namespace ApiTests.UnitTests
{
    public class DependentOverageCalculationRuleTest : CalculationRulesTest
    {

        [Theory]
        [InlineData(0, true)]
        [InlineData(0, false)]
        public void CheckEmployeeWithoutDependent(decimal expected, bool forTwicePaid)
        {
            CheckEmployee(expected, forTwicePaid, EmployeeWithoutDependent);
        }


        [Theory]
        [InlineData(0, true)]
        [InlineData(0, false)]
        public void CheckEmployeeWithYoungerDependent(decimal expected, bool forTwicePaid)
        {
            CheckEmployee(expected, forTwicePaid, EmployeeWithSpouse);
        }

        [Theory]
        [InlineData(100, true)]
        [InlineData(66.67, false)]
        public void CheckEmployeeWithOlderDependent(decimal expected, bool forTwicePaid)
        {
            var spouse = EmployeeWithSpouse.Dependents.First();
            var dependentBirthDate = spouse.DateOfBirth;
            spouse.DateOfBirth = DateTime.Today.AddYears(-70);
            CheckEmployee(expected, forTwicePaid, EmployeeWithSpouse);
            spouse.DateOfBirth = dependentBirthDate;
        }


        [Theory]
        [InlineData(100, 1, 2, 3, 4, 5)]
        public void CheckEmployeeWithDependentTurning50AtFirstJune(decimal expected, params int[] weeks)
        {
            CheckEmployeeDependentTurning50(expected, weeks, 1);

        }

        [Theory]
        [InlineData(100, 1, 2, 3, 4, 5)]
        public void CheckEmployeeWithDependentTurning50AtTenthJune(decimal expected, params int[] weeks)
        {
            CheckEmployeeDependentTurning50(expected, weeks, 10);
        }


        [Theory]
        [InlineData(0, 1)]
        [InlineData(200, 2)]
        [InlineData(100, 3, 4, 5)]
        public void CheckEmployeeWithDependentTurning50AtTwentiehJune(decimal expected, params int[] weeks)
        {
            CheckEmployeeDependentTurning50(expected, weeks, 20);
        }

        [Theory]
        [InlineData(0, 1, 2)]
        [InlineData(200, 3, 4)]
        [InlineData(100, 5)]
        public void CheckEmployeeWithDependentTurning50AtThirtiehJune(decimal expected, params int[] weeks)
        {
            CheckEmployeeDependentTurning50(expected, weeks, 30);
        }


        private void CheckEmployee(decimal expected, bool forTwicePaid, Employee employee)
        {
            var rule = new DependentsOverAgeRule
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

        private void CheckEmployeeDependentTurning50(decimal expected, int[] weeks, int day)
        {
            var rule = new DependentsOverAgeRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };

            var spouse = EmployeeWithSpouse.Dependents.First();
            var dependentBirthDate = spouse.DateOfBirth;

            spouse.DateOfBirth = new DateTime(ReferenceYear - 50, 1, day);

            foreach (var weekNo in weeks)
            {
                if (!rule.Eligible(EmployeeWithSpouse, weekNo)) continue;

                var effect = rule.Effect(EmployeeWithSpouse, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
            spouse.DateOfBirth = dependentBirthDate;
        }

    }
}
