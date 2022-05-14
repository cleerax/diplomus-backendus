using static DiplomusContractors.Products.Product;

namespace DiplomusContractors.Products;

public record Product(int Id, string Name, ProductStatus Status, ProductContractor[] Contractors)
{
    public record ProductContractor(int Id, string Name, string Email, string? Inn, string? Address, string? ImageLink, decimal Price);
}
