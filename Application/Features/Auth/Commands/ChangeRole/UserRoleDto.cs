// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Application.Features.Auth.Commands.ChangeRole
{
	public class UserRoleDto
	{
		public long UserId { get; set; }
		public string Role { get; set; } = null!;
	}
}
