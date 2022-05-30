using DiplomusContractors.Products;

namespace DiplomusContractors.Repositories.Products;

public record Product(int ProductId, string ProductName, ProductStatus ProductStatus, Category? Category, bool IsAvailable);
