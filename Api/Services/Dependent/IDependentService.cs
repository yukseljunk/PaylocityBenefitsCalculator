using ErrorOr;

namespace Api.Services;

public interface IDependentService
{
    Task<ErrorOr<Created>> CreateDependent(Models.Dependent dependent);

    Task<ErrorOr<Deleted>> DeleteDependent(int id);

    Task<ErrorOr<Models.Dependent>> GetDependent(int id);

    Task<ErrorOr<Updated>> Update(Models.Dependent dependent);

    Task<ErrorOr<List<Models.Dependent>>> GetDependents();

}
