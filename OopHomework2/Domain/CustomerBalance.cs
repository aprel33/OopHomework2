
namespace Marketplace.Domain;

public struct CustomerBalance
{
    public decimal Money { get; init; }
    public decimal Bonuses { get; init; }
    public decimal AllowedBonusShare { get; init; }

    public CustomerBalance(
        decimal money,
        decimal bonuses,
        decimal allowedBonusShare)
    {
        ValidateMoney(money);
        ValidateBonuses(bonuses);
        ValidateAllowedBonusShare(allowedBonusShare);

        Money = money;
        Bonuses = bonuses;
        AllowedBonusShare = allowedBonusShare;
    }

    private static void ValidateMoney(decimal money)
    {
        if (money < 0)
            throw new ArgumentOutOfRangeException(
                nameof(money),
                "Денежный баланс не может быть отрицательным.");
    }

    private static void ValidateBonuses(decimal bonuses)
    {
        if (bonuses < 0)
            throw new ArgumentOutOfRangeException(
                nameof(bonuses),
                "Бонусный баланс не может быть отрицательным.");
    }

    private static void ValidateAllowedBonusShare(decimal share)
    {
        if (share < 0 || share > 0.5m)
            throw new ArgumentOutOfRangeException(
                nameof(share),
                "Доля оплаты бонусами должна быть от 0 до 0.5.");
    }
}
