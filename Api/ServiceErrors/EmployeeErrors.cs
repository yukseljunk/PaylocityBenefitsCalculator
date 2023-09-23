using ErrorOr;

namespace Api.ServiceErrors;

public static class EmployeeErrors
{
    public static Error NotFound => Error.NotFound("Employee.NotFound", "Employee not found with given id");
}
