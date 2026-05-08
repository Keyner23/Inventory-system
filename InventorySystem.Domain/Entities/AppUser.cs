using Microsoft.AspNetCore.Identity;

namespace InventorySystem.Domain.Entities;

public class AppUser:IdentityUser<Guid>
{
    public Guid CompanyId { get; set; }

    public Company Company { get; set; } = null!;
}