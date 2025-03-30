using DomainCommons.EntityStronglyIds;

namespace ChatService.Domain.Dtos;
public record UserDto
{
    public UserId Id { get; set; }
    public required string Name { get; set; }
    public required Uri? Avatar { get; set; }
}
