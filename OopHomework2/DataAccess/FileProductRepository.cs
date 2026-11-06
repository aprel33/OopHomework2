using System;
using System.Collections.Generic;
using Marketplace.Domain;

namespace Marketplace.Presentation;

public class CatalogPrinter
{
    // Вывод всех товаров.
    public void PrintAll(IEnumerable<Product> products)
    {
        // TODO участника 3:
        // Перебрать товары и вывести каждый через PrepareLine.
        throw new NotImplementedException();
    }

    // Вывод самых дорогих товаров.
    public void PrintTop(IEnumerable<Product> products, int count)
    {
        // TODO участника 3:
        // Отсортировать товары по цене по убыванию
        // и вывести count самых дорогих.
        throw new NotImplementedException();
    }

    // Подготовка строки с информацией о товаре.
    private string PrepareLine(Product product)
    {
        // TODO участника 3.
        throw new NotImplementedException();
    }
}

// Вспомогательный тип виден только в этом файле.
file static class PriceFormatter
{
    public static string Format(int price) => $"{price} у.е.";
}