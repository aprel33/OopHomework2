
using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Domain;

namespace Marketplace.Presentation;

public class CatalogReport
{
    public decimal GetAveragePrice(IEnumerable<Product> products)
    {
        if (products == null)
            throw new ArgumentNullException(nameof(products));

        var productList = products.ToList();

        if (productList.Count == 0)
            return 0m;

        return productList.Average(p => (decimal)p.GetPrice());
    }

    public string GetSummary(IEnumerable<Product> products)
    {
        if (products == null)
            throw new ArgumentNullException(nameof(products));

        var productList = products.ToList();

        if (productList.Count == 0)
            return "Каталог пуст.";

        int count = productList.Count;
        decimal averagePrice = GetAveragePrice(productList);
        int minPrice = productList.Min(p => p.GetPrice());
        int maxPrice = productList.Max(p => p.GetPrice());

        return $"Количество товаров: {count}\n" +
               $"Средняя цена: {averagePrice:F2} у.е.\n" +
               $"Минимальная цена: {minPrice} у.е.\n" +
               $"Максимальная цена: {maxPrice} у.е.";
    }
}
