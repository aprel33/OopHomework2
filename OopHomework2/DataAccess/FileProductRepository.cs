using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Marketplace.Domain;

namespace Marketplace.DataAccess;

public class FileProductRepository : IProductRepository
{
    private readonly string filePath;

    public FileProductRepository(string filePath = "products.json")
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException(
                "Путь к файлу не может быть пустым.",
                nameof(filePath));

        this.filePath = filePath;
    }

    // Сохранение товаров в файл.
    public void Save(IEnumerable<Product> products)
    {
        if (products == null)
            throw new ArgumentNullException(nameof(products));

        string json = Serialize(products);

        string? directory = Path.GetDirectoryName(
            Path.GetFullPath(filePath));

        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllText(filePath, json);
    }

    // Загрузка товаров из файла.
    public List<Product> Load()
    {
        if (!File.Exists(filePath))
            return new List<Product>();

        string json = File.ReadAllText(filePath);

        return Deserialize(json);
    }

    // Преобразование товаров в JSON.
    private string Serialize(IEnumerable<Product> products)
    {
        var data = products.Select(product => new ProductData
        {
            Id = product.GetId(),
            Title = product.GetTitle(),
            Price = product.GetPrice()
        }).ToList();

        return JsonSerializer.Serialize(
            data,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });
    }

    // Восстановление товаров из JSON.
    private List<Product> Deserialize(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<Product>();

        try
        {
            List<ProductData>? data =
                JsonSerializer.Deserialize<List<ProductData>>(json);

            if (data == null)
                return new List<Product>();

            return data.Select(item =>
                new Product(item.Id, item.Title, item.Price)
            ).ToList();
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException(
                "Ошибка чтения каталога из JSON-файла.", ex);
        }
    }

    // Промежуточная модель для JSON.
    private class ProductData
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int Price { get; set; }
    }
}
