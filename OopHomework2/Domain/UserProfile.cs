
namespace Marketplace.Domain;

public class UserProfile
{
    private readonly List<CartItem> _completedPurchases = new();

    public int Id { get; }
    public string Name { get; }

    public decimal MoneyBalance { get; private set; }
    public decimal BonusBalance { get; private set; }

    // Множитель 0.95 означает скидку 5%.
    public decimal PersonalDiscount { get; private set; }

    // Например, 0.1 означает, что бонусами можно оплатить до 10%.
    public decimal AllowedBonusShare { get; private set; }

    public IReadOnlyList<CartItem> CompletedPurchases =>
        _completedPurchases.AsReadOnly();

    public UserProfile(
        int id,
        string name,
        decimal moneyBalance = 0m,
        decimal bonusBalance = 0m,
        decimal personalDiscount = 1m,
        decimal allowedBonusShare = 0m)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Имя пользователя не может быть пустым.", nameof(name));

        ValidateNonNegative(moneyBalance, nameof(moneyBalance));
        ValidateNonNegative(bonusBalance, nameof(bonusBalance));
        ValidateDiscount(personalDiscount);
        ValidateBonusShare(allowedBonusShare);

        Id = id;
        Name = name;
        MoneyBalance = moneyBalance;
        BonusBalance = bonusBalance;
        PersonalDiscount = personalDiscount;
        AllowedBonusShare = allowedBonusShare;
    }

    public void AddMoney(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount), "Сумма пополнения должна быть положительной.");

        MoneyBalance += amount;
    }

    public void AddBonuses(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount), "Количество бонусов должно быть положительным.");

        BonusBalance += amount;
    }

    public void SetPersonalDiscount(decimal multiplier)
    {
        ValidateDiscount(multiplier);
        PersonalDiscount = multiplier;
    }

    public void SetAllowedBonusShare(decimal share)
    {
        ValidateBonusShare(share);
        AllowedBonusShare = share;
    }

    public CustomerBalance GetCustomerBalance() =>
        new(MoneyBalance, BonusBalance, AllowedBonusShare);

    public void ApplyCustomerBalance(CustomerBalance balance)
    {
        // Проверяем всё до изменения состояния пользователя.
        ValidateNonNegative(balance.Money, nameof(balance));
        ValidateNonNegative(balance.Bonuses, nameof(balance));
        ValidateBonusShare(balance.AllowedBonusShare);

        MoneyBalance = balance.Money;
        BonusBalance = balance.Bonuses;
        AllowedBonusShare = balance.AllowedBonusShare;
    }

    public void AddCompletedPurchases(IEnumerable<CartItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        // Создаём независимые копии позиций, чтобы последующие изменения
        // корзины не меняли историю покупок.
        var copies = items.Select(item => item.Copy()).ToList();

        _completedPurchases.AddRange(copies);
    }

    private static void ValidateNonNegative(decimal value, string parameterName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(
                parameterName, "Значение не может быть отрицательным.");
    }

    private static void ValidateDiscount(decimal multiplier)
    {
        if (multiplier < 0.01m || multiplier > 1m)
            throw new ArgumentOutOfRangeException(
                nameof(multiplier), "Множитель скидки должен быть от 0.01 до 1.00.");
    }

    private static void ValidateBonusShare(decimal share)
    {
        if (share < 0m || share > 0.5m)
            throw new ArgumentOutOfRangeException(
                nameof(share), "Доля оплаты бонусами должна быть от 0 до 0.5.");
    }
}
