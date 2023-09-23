using ErrorOr;

namespace Api.ServiceErrors;

public static class DependentErrors
{
    public static Error NotFound => Error.NotFound("Dependent.NotFound", "Dependent not found with given id");
}
