using ErrorOr;

namespace Api.ServiceErrors;

public static class DependentErrors
{

    public static Error NotFound(int id) => Error.NotFound("Dependent.NotFound", $"Dependent '{id}' not found");
    public static Error NoRelationshipDependent(string fullName) => Error.Validation("Dependent.NoRelationshipDependent", $"Dependent '{fullName}' should have a relation type specified");
    public static Error InvalidRelationshipDependent(string fullName) => Error.Validation("Dependent.InvalidRelationshipDependent", $"Dependent '{fullName}' should have a relation type 0 to 3");
    public static Error DuplicateId(int[] ids) => Error.Validation("Dependent.DuplicateId", $"Id(s) '{string.Join(",", ids)}' are duplicated.");
    public static Error BirthdayBeforeToday => Error.NotFound("Employee.BirthdayBeforeToday", $"Employee cannot have birthday today or in future");


}
