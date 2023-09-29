using Api.Calculation.CalculationRules;
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

        [Theory]
        [InlineData(0, true)]
        [InlineData(0, false)]
        public void CheckEmployeeWithYoungerDependent(decimal expected, bool forTwicePaid)
        {
            var rule = new DependentsOverAgeRule
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

        [Theory]
        [InlineData(100, true)]
        [InlineData(66.67, false)]
        public void CheckEmployeeWithOlderDependent(decimal expected, bool forTwicePaid)
        {
            var rule = new DependentsOverAgeRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };
            var reflist = forTwicePaid ? WeekNumbersForTwicePaidMonth : WeekNumbersForThricePaidMonth;
            var spouse = EmployeeWithSpouse.Dependents.First();
            var dependentBirthDate = spouse.DateOfBirth;
            spouse.DateOfBirth = DateTime.Today.AddYears(-70);
            foreach (var weekNo in reflist)
            {
                if (!rule.Eligible(EmployeeWithSpouse, weekNo)) continue;

                var effect = rule.Effect(EmployeeWithSpouse, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
            spouse.DateOfBirth = dependentBirthDate;

        }



        [Theory]
        [InlineData(100, 1, 2, 3, 4, 5)]
        public void CheckEmployeeWithDependentTurning50AtFirstJune(decimal expected, params int[] weeks)
        {
            var rule = new DependentsOverAgeRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };

            var spouse = EmployeeWithSpouse.Dependents.First();
            var dependentBirthDate = spouse.DateOfBirth;

            spouse.DateOfBirth = new DateTime(ReferenceYear - 50, 1, 1);

            foreach (var weekNo in weeks)
            {
                if (!rule.Eligible(EmployeeWithSpouse, weekNo)) continue;

                var effect = rule.Effect(EmployeeWithSpouse, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
            spouse.DateOfBirth = dependentBirthDate;

        }


        [Theory]
        [InlineData(100, 1, 2, 3, 4, 5)]
        public void CheckEmployeeWithDependentTurning50AtTenthJune(decimal expected, params int[] weeks)
        {
            var rule = new DependentsOverAgeRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };

            var spouse = EmployeeWithSpouse.Dependents.First();
            var dependentBirthDate = spouse.DateOfBirth;

            spouse.DateOfBirth = new DateTime(ReferenceYear - 50,1, 10);

            foreach (var weekNo in weeks)
            {
                if (!rule.Eligible(EmployeeWithSpouse, weekNo)) continue;

                var effect = rule.Effect(EmployeeWithSpouse, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
            spouse.DateOfBirth = dependentBirthDate;

        }


        [Theory]
        [InlineData(0, 1)]
        [InlineData(200, 2)]
        [InlineData(100, 3, 4, 5)]
        public void CheckEmployeeWithDependentTurning50AtTwentiehJune(decimal expected, params int[] weeks)
        {
            var rule = new DependentsOverAgeRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };

            var spouse = EmployeeWithSpouse.Dependents.First();
            var dependentBirthDate = spouse.DateOfBirth;

            spouse.DateOfBirth = new DateTime(ReferenceYear - 50, 1, 20);

            foreach (var weekNo in weeks)
            {
                if (!rule.Eligible(EmployeeWithSpouse, weekNo)) continue;

                var effect = rule.Effect(EmployeeWithSpouse, weekNo);
                Assert.Equal(expected, Decimal.Round(effect, 2));

            }
            spouse.DateOfBirth = dependentBirthDate;

        }

        [Theory]
        [InlineData(0, 1,2)]
        [InlineData(200, 3, 4)]
        [InlineData(100, 5)]
        public void CheckEmployeeWithDependentTurning50AtThirtiehJune(decimal expected, params int[] weeks)
        {
            var rule = new DependentsOverAgeRule
            {
                WeeksForYear = WpCalculation.Item1,
                PaycheckCountForMonth = WpCalculation.Item2
            };

            var spouse = EmployeeWithSpouse.Dependents.First();
            var dependentBirthDate = spouse.DateOfBirth;

            spouse.DateOfBirth = new DateTime(ReferenceYear - 50, 1, 30);

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
