using Api.Dtos.Dependent;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Employee;

public class GetEmployeeDto
{
    public int Id { get; set; }

    [MinLength(2)]
    [MaxLength(20)]
    public string? FirstName { get; set; }

    [MinLength(2)]
    [MaxLength(20)]
    public string? LastName { get; set; }

    [Range(0, double.MaxValue, ErrorMessage ="Salary cannot be negative.")]
    public decimal Salary { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
    public ICollection<GetDependentDto> Dependents { get; set; } = new List<GetDependentDto>();
}
