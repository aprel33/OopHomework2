namespace Marketplace.Domain;

public class CartItem
{
    public int Id { get; }
    public int ProductId { get; }
    public int Quantity { get; private set; }

    public CartItem(int id, int productId, int quantity)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id), "ID позиции должен быть больше нуля.");

        if (productId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(productId), "ID товара должен быть больше нуля.");

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Количество должно быть больше нуля.");

        Id = id;
        ProductId = productId;
        Quantity = quantity;
    }

    public void ChangeQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Количество должно быть больше нуля.");

        Quantity = quantity;
    }

    public CartItem Copy()
    {
        return new CartItem(Id, ProductId, Quantity);
    }
}
