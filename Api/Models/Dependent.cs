namespace Api.Models;

public class Dependent
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (today.Month < DateOfBirth.Month || (today.Month == DateOfBirth.Month && today.Day < DateOfBirth.Day))
                age--;
            return age;

        }
    }
}
