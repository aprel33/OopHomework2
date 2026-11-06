namespace Marketplace.Domain;

public class Product
{
    // Закрытые поля
    private int id;
    private string title = string.Empty;
    private int price;

    public Product(int id, string title, int price)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id),
                "ID товара должен быть больше нуля.");

        EnsureTitle(title);
        EnsurePrice(price);

        this.id = id;
        this.title = title;
        this.price = price;
    }

    // Методы чтения
    public int GetId() => id;

    public string GetTitle() => title;

    public int GetPrice() => price;

    // Изменение цены с проверкой
    public void SetPrice(int newPrice)
    {
        EnsurePrice(newPrice);
        price = newPrice;
    }

    // Информация о товаре
    public string GetInfo() =>
        $"Товар: {title}, ID: {id}, цена: {price} у.е.";

    private static void EnsureTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Название товара не должно быть пустым.",
                nameof(value));
    }

    private static void EnsurePrice(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "Цена не может быть отрицательной.");
    }
}