using static DiplomusContractors.Products.Product;

namespace DiplomusContractors.Products;

public record Product(int Id, string Name, Category? Category, ProductStatus Status, ProductContractor[] Contractors, bool IsAvailable)
{
    public record ProductContractor(
        int Id,
        string Name,
        string Email,
        string? Inn,
        string? Address,
        string? ImageLink,
        decimal Price,
        decimal? DeliveryPrice,
        decimal MarketPrice,
        decimal Price1,
        decimal Price2,
        decimal Price3);
}
