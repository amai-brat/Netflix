using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppRole(string roleName) : IdentityRole<long>(roleName);