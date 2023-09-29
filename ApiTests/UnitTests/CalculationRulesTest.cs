using Api.Models;
using Api.Services;
using System;
using System.Collections.Generic;

namespace ApiTests.UnitTests
{
    public class CalculationRulesTest
    {
        private Tuple<Dictionary<int, Tuple<DateTime, DateTime>>, Dictionary<int, int>> _wpCalculation;
        protected List<int> WeekNumbersForTwicePaidMonth = new List<int>();
        protected List<int> WeekNumbersForThricePaidMonth = new List<int>();
        protected Employee EmployeeWithoutDependent = Employee.Create("John", "Doe", 26000, DateTime.Today.AddYears(-30), new List<Dependent>()).Value;
        protected Employee EmployeeWithSpouse = Employee.Create("Matthew", "Brown", 26000, DateTime.Today.AddYears(-35), new List<Dependent>() { 
            Dependent.Create("Susan","Brown", DateTime.Today.AddYears(-32), Relationship.Spouse).Value
                }).Value;
        protected int ReferenceYear => 2023;

        protected Tuple<Dictionary<int, Tuple<DateTime, DateTime>>, Dictionary<int, int>> WpCalculation
        {

            get
            {
                if (_wpCalculation == null)
                {
                    _wpCalculation = BonusService.CalculateWeeksAndPaychecks(new DateTime(ReferenceYear, 1, 1));
                    var weeksForYear = _wpCalculation.Item1;
                    var paycheckCountForMonth = _wpCalculation.Item2;
                    var weekCounter = 1;
                    foreach (var paycheckCount in paycheckCountForMonth)
                    {
                        if (paycheckCount.Value == 2)
                        {
                            for (int i = 0; i < paycheckCount.Value; i++)
                            {
                                WeekNumbersForTwicePaidMonth.Add(weekCounter);
                                weekCounter++;

                            }
                        }
                        else//3
                        {
                            for (int i = 0; i < paycheckCount.Value; i++)
                            {
                                WeekNumbersForThricePaidMonth.Add(weekCounter);
                                weekCounter++;

                            }
                        }
                    }

                }
                return _wpCalculation;
            }

        }
    }
}
