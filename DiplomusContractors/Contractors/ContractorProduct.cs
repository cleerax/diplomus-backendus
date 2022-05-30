using DiplomusContractors.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomusContractors.Contractors;

public record ContractorProduct(int Id, string Name, Category? Category, ProductStatus Status, decimal Price, decimal DeliveryPrice, decimal MarketPrice, bool IsAvailable);
