using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Domain;

public class ShoppingCart
{
    private readonly List<CartItem> _items = new();

    public int Id { get; }
    public int UserId { get; }
    public IReadOnlyList<CartItem> Items => _items;

    public ShoppingCart(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), "ID корзины должен быть положительным.");
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId), "ID пользователя должен быть положительным.");

        Id = id;
        UserId = userId;
    }
    public void AddItem(CartItem item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item), "Позиция не может быть null.");

        if (_items.Any(i => i.Id == item.Id))
            throw new InvalidOperationException($"Позиция с ID {item.Id} уже есть в корзине.");

        _items.Add(item);
    }
    public bool RemoveItem(int itemId)
    {
        int index = _items.FindIndex(i => i.Id == itemId);
        if (index < 0)
            return false;

        _items.RemoveAt(index);
        return true;
    }

    public void Clear()
    {
        _items.Clear();
    }
}
