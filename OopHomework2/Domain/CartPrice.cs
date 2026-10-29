using System;

namespace Marketplace.Domain;

public readonly struct CartPrice
{
    public decimal OriginalPrice { get; }
    public decimal DiscountMultiplier { get; }
    public decimal DiscountAmount { get; }
    public decimal FinalPrice { get; }

    public CartPrice(decimal originalPrice, decimal discountMultiplier)
    {
        if (originalPrice < 0m)
            throw new ArgumentOutOfRangeException(nameof(originalPrice), "Исходная цена не может быть отрицательной.");

        if (discountMultiplier < 0.01m || discountMultiplier > 1.00m)
            throw new ArgumentOutOfRangeException(nameof(discountMultiplier), "Множитель скидки должен быть от 0.01 до 1.00.");

        OriginalPrice = originalPrice;
        DiscountMultiplier = discountMultiplier;
        DiscountAmount = originalPrice * (1m - discountMultiplier);
        FinalPrice = originalPrice - DiscountAmount;
    }
    public string ShowFinalPrice() => $"Итоговая цена: {FinalPrice:F2} УЕ";
    public string ShowDiscountAmount() => $"Размер скидки: {DiscountAmount:F2} УЕ";
    public override string ToString() =>
        $"Исходная цена: {OriginalPrice:F2} УЕ, множитель: {DiscountMultiplier:F2}, " +
        $"скидка: {DiscountAmount:F2} УЕ, итог: {FinalPrice:F2} УЕ";
}
