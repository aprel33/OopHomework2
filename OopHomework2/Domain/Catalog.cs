using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Domain;

public class Catalog
{
    // Закрытая коллекция товаров.
    private readonly List<Product> products = new();

    // Добавление товара.
    public void Add(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        if (products.Any(p => SameTitle(p.GetTitle(), product.GetTitle())))
            throw new InvalidOperationException(
                "Товар с таким названием уже существует.");

        products.Add(product);
    }

    // Удаление товара по названию.
    public bool Remove(string title)
    {
        Product? product = Find(title);

        if (product == null)
            return false;

        return products.Remove(product);
    }

    // Поиск товара по названию.
    public Product? Find(string title)
    {
        return products.FirstOrDefault(
            p => SameTitle(p.GetTitle(), title));
    }

    // Возвращение копии коллекции.
    public IReadOnlyList<Product> GetProducts()
    {
        return products.ToList();
    }

    // Получение количества товаров.
    public int GetCount()
    {
        return products.Count;
    }

    // Нормализация названия.
    private static string Normalize(string title)
    {
        return title.Trim().ToLowerInvariant();
    }

    // Сравнение названий без учёта регистра и пробелов по краям.
    private static bool SameTitle(string left, string right)
    {
        return Normalize(left) == Normalize(right);
    }
}