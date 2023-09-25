using ErrorOr;

namespace Api.ServiceErrors;

public static class DependentErrors
{
    public static Error NotFound => Error.NotFound("Dependent.NotFound", "Dependent not found with given id");
    public static Error NoRelationshipDependent => Error.Validation("Dependent.NoRelationshipDependent", "Dependent should have a relation type specified");
    public static Error DuplicateId => Error.Validation("Dependent.DuplicateId", "Two or more dependants have same id");

}
