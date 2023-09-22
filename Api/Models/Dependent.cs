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
}
