using Api.Calculation.CalculationRules;
using Api.Models;
using System;
using System.Linq;
using Xunit;

namespace ApiTests.UnitTests
{
    public class KidsCalculationRuleTest : CalculationRulesTest
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
        public void CheckEmployeeWithAdultKid(decimal expected, bool forTwicePaid)
        {
            var kid = EmployeeWithSpouse.Dependents.First();
            var dependentBirthDate = kid.DateOfBirth;
            kid.DateOfBirth = DateTime.Today.AddYears(-25);
            CheckEmployee(expected, forTwicePaid, EmployeeWithKid);
            kid.DateOfBirth = dependentBirthDate;
        }

        [Theory]
        [InlineData(100, true)]
        [InlineData(66.67, false)]
        public void CheckEmployeeWithSmallKid(decimal expected, bool forTwicePaid)
        {
            CheckEmployee(expected, forTwicePaid, EmployeeWithKid);
        }


        [Theory]
        [InlineData(100, 1, 2, 3, 4, 5)]
        public void CheckEmployeeWithKidBornAtFirstJune(decimal expected, params int[] weeks)
        {
            CheckEmployeeWithKidBorn(expected, weeks, 1);

        }

        [Theory]
        [InlineData(100, 1, 2, 3, 4, 5)]
        public void CheckEmployeeWithKidBornAtTenthJune(decimal expected, params int[] weeks)
        {
            CheckEmployeeWithKidBorn(expected, weeks, 10);
        }


        [Theory]
        [InlineData(0, 1)]
        [InlineData(200, 2)]
        [InlineData(100, 3, 4, 5)]
        public void CheckEmployeeWithKidBornAtTwentiehJune(decimal expected, params int[] weeks)
        {
            CheckEmployeeWithKidBorn(expected, weeks, 20);
        }

        [Theory]
        [InlineData(0, 1, 2)]
        [InlineData(200, 3, 4)]
        [InlineData(100, 5)]
        public void CheckEmployeeWithKidBornAtThirtiehJune(decimal expected, params int[] weeks)
        {
            CheckEmployeeWithKidBorn(expected, weeks, 30);
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
                if (!rule.Eligible(employee, weekNo)) continue;

                var effect = rule.Effect(employee, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
        }

        private void CheckEmployeeWithKidBorn(decimal expected, int[] weeks, int day)
        {
            var rule = new DependentsOverAgeRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };

            var kid = EmployeeWithKid.Dependents.First();
            var dependentBirthDate = kid.DateOfBirth;

            kid.DateOfBirth = new DateTime(ReferenceYear, 1, day);

            foreach (var weekNo in weeks)
            {
                if (!rule.Eligible(EmployeeWithKid, weekNo)) continue;

                var effect = rule.Effect(EmployeeWithKid, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
            kid.DateOfBirth = dependentBirthDate;
        }

    }
}
