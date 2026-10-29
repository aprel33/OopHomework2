
using Marketplace.Domain;

namespace Marketplace.Services;

public class PaymentService
{
    public bool TryPay(
        UserProfile customer,
        ShoppingCart cart,
        CustomerBalance balance,
        CartPrice price,
        out CustomerBalance updatedBalance,
        out string message)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(cart);

        updatedBalance = balance;

        if (cart.UserId != customer.Id)
        {
            message = "Ошибка: корзина принадлежит другому пользователю.";
            return false;
        }

        if (cart.Items.Count == 0)
        {
            message = "Корзина пуста.";
            return false;
        }

        if (balance.Money != customer.MoneyBalance ||
            balance.Bonuses != customer.BonusBalance ||
            balance.AllowedBonusShare != customer.AllowedBonusShare)
        {
            message = "Ошибка: переданный баланс устарел.";
            return false;
        }

        decimal finalPrice = price.FinalPrice;

        if (finalPrice <= 0m)
        {
            message = "Стоимость заказа должна быть больше нуля.";
            return false;
        }

        decimal maxBonusPayment =
            finalPrice * balance.AllowedBonusShare;

        decimal bonusesToUse =
            Math.Min(balance.Bonuses, maxBonusPayment);

        decimal moneyToUse = finalPrice - bonusesToUse;

        if (balance.Money < moneyToUse)
        {
            message =
                $"Недостаточно средств. Нужно денег: {moneyToUse:F2} УЕ, " +
                $"доступно: {balance.Money:F2} УЕ.";

            return false;
        }

        // Создаём новую копию структуры через with.
        updatedBalance = balance with
        {
            Money = balance.Money - moneyToUse,
            Bonuses = balance.Bonuses - bonusesToUse
        };

        // Подготавливаем копии покупок до изменения состояния.
        var purchasedItems = cart.Items
            .Select(item => item.Copy())
            .ToList();

        customer.ApplyCustomerBalance(updatedBalance);
        customer.AddCompletedPurchases(purchasedItems);
        cart.Clear();

        message =
            $"Оплата прошла успешно. Списано деньгами: {moneyToUse:F2} УЕ, " +
            $"бонусами: {bonusesToUse:F2} УЕ.";

        return true;
    }
}
