using InventorySystem.Domain.Common;

namespace InventorySystem.Domain.Entities;

public class Company: BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}