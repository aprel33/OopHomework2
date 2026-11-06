
using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Domain;

namespace Marketplace.Presentation;

public class CatalogPrinter
{
    public void PrintAll(IEnumerable<Product> products)
    {
        if (products == null)
            throw new ArgumentNullException(nameof(products));

        Console.WriteLine("=== Все товары каталога ===");

        foreach (Product product in products)
        {
            Console.WriteLine(PrepareLine(product));
        }
    }

    public void PrintTop(IEnumerable<Product> products, int count)
    {
        if (products == null)
            throw new ArgumentNullException(nameof(products));

        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        Console.WriteLine($"=== Топ-{count} дорогих товаров ===");

        var topProducts = products
            .OrderByDescending(p => p.GetPrice())
            .Take(count);

        foreach (Product product in topProducts)
        {
            Console.WriteLine(PrepareLine(product));
        }
    }

    private string PrepareLine(Product product)
    {
        return $"ID: {product.GetId()} | " +
               $"Название: {product.GetTitle()} | " +
               $"Цена: {PriceFormatter.Format(product.GetPrice())}";
    }
}

file static class PriceFormatter
{
    public static string Format(int price)
    {
        return $"{price:N0} у.е.";
    }
}
