using System;
using System.Collections.Generic;
using Marketplace.Domain;

namespace Marketplace.Presentation;

public class CatalogReport
{
    // Расчёт средней стоимости товаров.
    public decimal GetAveragePrice(IEnumerable<Product> products)
    {
        // TODO участника 3:
        // Рассчитать среднюю цену товаров.
        throw new NotImplementedException();
    }

    // Формирование текстовой сводки каталога.
    public string GetSummary(IEnumerable<Product> products)
    {
        // TODO участника 3:
        // Подготовить сводку с количеством товаров
        // и информацией о ценах.
        throw new NotImplementedException();
    }
}