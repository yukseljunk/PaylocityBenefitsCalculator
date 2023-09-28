using Api.Calculation.CalculationRules;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using ErrorOr;

namespace Api.Mappers
{
    public class ModelToDtoMapper
    {
        public static GetDependentDto MapDependentResponse(Dependent dependent)
        {
            return new GetDependentDto()
            {
                Id = dependent.Id,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                DateOfBirth = dependent.DateOfBirth,
                Relationship = dependent.Relationship
            };

        }

        public static GetEmployeeDto MapEmployeeResponse(Employee employee)
        {
            var dependents = new List<GetDependentDto>();
            employee.Dependents.ToList().ForEach(dependent => dependents.Add(MapDependentResponse(dependent)));
            return new GetEmployeeDto()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                Dependents = dependents,
                Salary = employee.Salary
            };
        }

        internal static GetBonusDto MapBonusResponse(Bonus bonus)//Employee employee, int weekNo, Dictionary<ICalculationRule, decimal> details
        {
            var detailsConverted = new Dictionary<string, decimal>();
            bonus.Details?.ToList().ForEach(d => detailsConverted.Add(d.Key.Name, d.Value));
            return new GetBonusDto()
            {
                EmployeeId = bonus.Employee.Id,
                WeekNo = bonus.WeekNo,
                AmountToPay = bonus.Employee.BiWeeklySalary,
                Details = detailsConverted
            };
        }
    }
}
