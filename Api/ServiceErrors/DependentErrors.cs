using ErrorOr;

namespace Api.ServiceErrors;

public static class DependentErrors
{

    public static Error NotFound(int id) => Error.NotFound("Dependent.NotFound", $"Dependent '{id}' not found");
    public static Error NoRelationshipDependent(int[] ids) => Error.Validation("Dependent.NoRelationshipDependent", $"Dependent(s) '{string.Join(",", ids)}' should have a relation type specified");
    public static Error InvalidRelationshipDependent(int[] ids) => Error.Validation("Dependent.InvalidRelationshipDependent", $"Dependent(s) '{string.Join(",", ids)}' should have a relation type 0 to 3");
    public static Error DuplicateId(int[] ids) => Error.Validation("Dependent.DuplicateId", $"Id(s) '{string.Join(",", ids)}' are duplicated.");

}
