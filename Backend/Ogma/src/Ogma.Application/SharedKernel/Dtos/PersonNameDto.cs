namespace Ogma.Application.SharedKernel.Dtos;

public record PersonNameDto(string FirstName, string LastName)
{
    public string FullName => $"{FirstName} {LastName}";
}
