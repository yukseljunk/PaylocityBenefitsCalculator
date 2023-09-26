using ErrorOr;

namespace Api.ServiceErrors;

public static class EmployeeErrors
{
    public static Error NotFound(int id) => Error.NotFound("Employee.NotFound", $"Employee '{id}' not found");
    public static Error NotMoreThanOneSpouse => Error.NotFound("Employee.NoMoreThanOneDependent", $"Employee cannot have more than one spouse");
    public static Error BirthdayBeforeToday => Error.NotFound("Employee.BirthdayBeforeToday", $"Employee cannot have birthday today or in future");

}
