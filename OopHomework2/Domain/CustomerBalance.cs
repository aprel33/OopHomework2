
namespace Marketplace.Domain;

public readonly struct CustomerBalance
{
    public decimal Money { get; }
    public decimal Bonuses { get; }
    public decimal AllowedBonusShare { get; }

    public CustomerBalance(
        decimal money,
        decimal bonuses,
        decimal allowedBonusShare)
    {
        if (money < 0)
            throw new ArgumentOutOfRangeException(
                nameof(money), "Денежный баланс не может быть отрицательным.");

        if (bonuses < 0)
            throw new ArgumentOutOfRangeException(
                nameof(bonuses), "Бонусный баланс не может быть отрицательным.");

        if (allowedBonusShare < 0 || allowedBonusShare > 0.5m)
            throw new ArgumentOutOfRangeException(
                nameof(allowedBonusShare),
                "Доля оплаты бонусами должна быть от 0 до 0.5.");

        Money = money;
        Bonuses = bonuses;
        AllowedBonusShare = allowedBonusShare;
    }
}
