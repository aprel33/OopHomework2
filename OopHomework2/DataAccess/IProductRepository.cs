using System.Collections.Generic;
using Marketplace.Domain;

namespace Marketplace.DataAccess;

public interface IProductRepository
{
    // Сохранение товаров.
    void Save(IEnumerable<Product> products);

    // Загрузка товаров.
    List<Product> Load();
}