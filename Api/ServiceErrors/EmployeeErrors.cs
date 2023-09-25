using ErrorOr;

namespace Api.ServiceErrors;

public static class EmployeeErrors
{
    public static Error NotFound => Error.NotFound("Employee.NotFound", "Employee not found with given id");
    public static Error NotMoreThanOneSpouse => Error.NotFound("Employee.NoMoreThanOneDependent", "Employee cannot have more than one spouse");

}
