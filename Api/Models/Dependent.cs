using Api.ServiceErrors;
using ErrorOr;

namespace Api.Models;

public class Dependent
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship
    {
        get;
        set;
    }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public int Age(DateTime referenceDate)
    {
        var age = referenceDate.Year - DateOfBirth.Year;
        if (referenceDate.Month < DateOfBirth.Month || (referenceDate.Month == DateOfBirth.Month && referenceDate.Day < DateOfBirth.Day))
            age--;
        return age;

    }

    private Dependent(int id, string? firstName, string? lastName, DateTime dateOfBirth, Relationship relationship)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Relationship = relationship;
    }

    public static ErrorOr<Dependent> Create(string? firstName, string? lastName, DateTime dateOfBirth, Relationship relationship, int? id = 0)
    {
        //complex validations here
        List<Error> errors = new();
        if (dateOfBirth >= DateTime.Now)
        {
            errors.Add(EmployeeErrors.BirthdayBeforeToday);
        }
        var fullName = $"{firstName??""} {lastName ?? ""}";
        if (!Enum.IsDefined(typeof(Relationship), relationship))
        {
            errors.Add(DependentErrors.InvalidRelationshipDependent(fullName));
        }
        if (relationship==Relationship.None)
        {
            errors.Add(DependentErrors.NoRelationshipDependent(fullName));
        }

        if (errors.Any())
        {
            return errors;
        }

        return new Dependent(
            id ?? 0,
            firstName,
            lastName,
            dateOfBirth,
            relationship
            );

    }


}
