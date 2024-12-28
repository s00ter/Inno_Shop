using Microsoft.AspNetCore.Identity;

namespace Inno_Shop.DataAccess.Entities;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string? LastUpdatedBy { get; set; }
    
    public List<Product> Products { get; set; }
}