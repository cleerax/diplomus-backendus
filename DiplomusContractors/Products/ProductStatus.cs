namespace DiplomusContractors.Products;

public enum ProductStatus
{
    Available = 0,
    Ordered = 1,
    InStock = 2,
    OutOfStock = 3
}

public static class ProductStatusExtensions
{
    public static ProductStatus ConvertToProductStatus(this string status) =>
        status switch
        {
            "available" => ProductStatus.Available,
            "ordered" => ProductStatus.Ordered,
            "in_stock" => ProductStatus.InStock,
            "out_of_stock" => ProductStatus.OutOfStock,
            _ => throw new NotSupportedException()
        };
}
