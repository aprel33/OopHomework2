namespace Marketplace.Domain;

public class Product
{
    public int Id { get; }
    public string Name { get; }
    public decimal UnitPrice { get; }

    public Product(int id, string name, decimal unitPrice)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id), "ID товара должен быть больше нуля.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Название товара не должно быть пустым.", nameof(name));

        if (unitPrice < 0)
            throw new ArgumentOutOfRangeException(
                nameof(unitPrice), "Цена не может быть отрицательной.");

        Id = id;
        Name = name;
        UnitPrice = unitPrice;
    }
}
