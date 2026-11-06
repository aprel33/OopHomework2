using Marketplace.Domain;
using Marketplace.Services;

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

var customer = new UserProfile(
    id: 1,
    name: "Григорий",
    moneyBalance: 0m,
    bonusBalance: 0m,
    personalDiscount: 0.95m,
    allowedBonusShare: 0.10m);

Console.WriteLine("=== Интернет-магазин ===");

// Пополняем баланс и начисляем бонусы.
customer.AddMoney(10000m);
customer.AddBonuses(1000m);

Console.WriteLine($"Покупатель: {customer.Name}");
Console.WriteLine($"Денежный баланс: {customer.MoneyBalance:F2} УЕ");
Console.WriteLine($"Бонусный баланс: {customer.BonusBalance:F2} УЕ");
Console.WriteLine(
    $"Персональная скидка: {(1m - customer.PersonalDiscount) * 100m:F0}%");
Console.WriteLine();

// Демонстрация ссылочного типа.
Console.WriteLine("=== Ссылочные типы ===");

var sameCustomer = customer;
sameCustomer.AddMoney(100m);

Console.WriteLine(
    $"Баланс через customer: {customer.MoneyBalance:F2} УЕ");
Console.WriteLine(
    $"Баланс через sameCustomer: {sameCustomer.MoneyBalance:F2} УЕ");
Console.WriteLine();

// Создаём товары.
var plate = new Product(1, "Тарелка", 1000);
var cutlery = new Product(2, "Приборы", 2000);

// Демонстрация методов Product.
Console.WriteLine("=== Информация о товарах ===");
Console.WriteLine(plate.GetInfo());
Console.WriteLine(cutlery.GetInfo());
Console.WriteLine();

// Создаём основную корзину.
var cart = new ShoppingCart(1, customer.Id);

cart.AddItem(new CartItem(1, plate.GetId(), 3));
cart.AddItem(new CartItem(2, cutlery.GetId(), 2));

// Рассчитываем стоимость заказа.
decimal originalPrice =
    plate.GetPrice() * 3 + cutlery.GetPrice() * 2;

var price = new CartPrice(
    originalPrice,
    customer.PersonalDiscount);

Console.WriteLine("=== Состав заказа ===");
Console.WriteLine($"Тарелки: 3 x {plate.GetPrice():F2} УЕ");
Console.WriteLine($"Приборы: 2 x {cutlery.GetPrice():F2} УЕ");
Console.WriteLine($"Стоимость до скидки: {price.OriginalPrice:F2} УЕ");
Console.WriteLine(price.ShowDiscountAmount());
Console.WriteLine(price.ShowFinalPrice());
Console.WriteLine();

// Создаём сервис оплаты.
var paymentService = new PaymentService();

Console.WriteLine("=== Неудачная оплата и повторная попытка ===");

// Создаём одну корзину для обеих попыток.
var purchaseCart = new ShoppingCart(2, customer.Id);
purchaseCart.AddItem(new CartItem(3, plate.GetId(), 20));

var purchasePrice = new CartPrice(
    plate.GetPrice() * 20,
    customer.PersonalDiscount);

Console.WriteLine($"Стоимость заказа: {purchasePrice.FinalPrice:F2} УЕ");

CustomerBalance balance = customer.GetCustomerBalance();

// Первая попытка: денег недостаточно.
bool firstAttempt = paymentService.TryPay(
    customer,
    purchaseCart,
    balance,
    purchasePrice,
    out var balanceAfterFailure,
    out var failureMessage);

Console.WriteLine(failureMessage);
Console.WriteLine($"Денежный баланс: {customer.MoneyBalance:F2} УЕ");
Console.WriteLine($"Бонусный баланс: {customer.BonusBalance:F2} УЕ");
Console.WriteLine($"Позиций в корзине: {purchaseCart.Items.Count}");
Console.WriteLine();

// Пополняем баланс.
Console.WriteLine("=== Пополнение баланса ===");

customer.AddMoney(10000m);

Console.WriteLine(
    $"Денежный баланс после пополнения: {customer.MoneyBalance:F2} УЕ");
Console.WriteLine();

// Повторяем оплату той же корзины.
Console.WriteLine("=== Повторная попытка оплаты ===");

balance = customer.GetCustomerBalance();

bool secondAttempt = paymentService.TryPay(
    customer,
    purchaseCart,
    balance,
    purchasePrice,
    out var balanceAfterPayment,
    out var paymentMessage);

Console.WriteLine(paymentMessage);
Console.WriteLine($"Денежный баланс: {customer.MoneyBalance:F2} УЕ");
Console.WriteLine($"Бонусный баланс: {customer.BonusBalance:F2} УЕ");
Console.WriteLine($"Покупок в истории: {customer.CompletedPurchases.Count}");
Console.WriteLine($"Позиций в корзине: {purchaseCart.Items.Count}");
Console.WriteLine();

// Демонстрация структуры и оператора with.
Console.WriteLine("=== Тип значения CustomerBalance ===");

var modifiedBalance = balanceAfterPayment with
{
    AllowedBonusShare = 0m
};

Console.WriteLine(
    $"Доля бонусов в исходной структуре: " +
    $"{balanceAfterPayment.AllowedBonusShare:P0}");

Console.WriteLine(
    $"Доля бонусов в новой структуре: " +
    $"{modifiedBalance.AllowedBonusShare:P0}");

Console.WriteLine(
    $"Денежный баланс в исходной структуре: " +
    $"{balanceAfterPayment.Money:F2} УЕ");

Console.WriteLine(
    $"Денежный баланс в профиле: {customer.MoneyBalance:F2} УЕ");



Console.WriteLine("=== Проверка каталога ===");

var catalog = new Catalog();

catalog.Add(plate);
catalog.Add(cutlery);

Console.WriteLine($"Количество товаров: {catalog.GetCount()}");

var foundProduct = catalog.Find("тарелка");

Console.WriteLine(
    foundProduct != null
        ? foundProduct.GetInfo()
        : "Товар не найден.");

bool removed = catalog.Remove("Приборы");

Console.WriteLine($"Удаление товара: {removed}");
Console.WriteLine($"Товаров после удаления: {catalog.GetCount()}");


// Проверка защиты от дубликатов.
Console.WriteLine("=== Проверка дубликатов ===");

try
{
    catalog.Add(new Product(3, "  ТАРЕЛКА  ", 1500));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Дубликат отклонён: {ex.Message}");
}

Console.WriteLine($"Количество товаров: {catalog.GetCount()}");
Console.WriteLine();


Console.WriteLine("=== Проверка инкапсуляции ===");

var productsCopy = catalog.GetProducts();

Console.WriteLine($"Товаров в копии: {productsCopy.Count}");

// Изменяем полученную копию.
if (productsCopy is List<Product> mutableCopy)
{
    mutableCopy.Clear();
}

Console.WriteLine($"Товаров в каталоге: {catalog.GetCount()}");