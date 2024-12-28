namespace Inno_Shop.DataAccess.Entities;

public class Product
{
    public string Id { get; init; }
    public string UserId { get; init; }
    public string Title { get; set; }
    public float Cost { get; set; }
    public string? Description { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string LastUpdatedBy { get; set; }

    public User User { get; set; }
}