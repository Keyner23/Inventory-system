using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(AppUser user);
}