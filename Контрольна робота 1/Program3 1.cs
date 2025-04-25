using System;


public class Fuel
{
    public string Brand { get; set; }
    public decimal PriceForLiter { get; set; }

    public override string ToString()
    {
        return $"{Brand} ({PriceForLiter} per liter)";
    }
}

public class PieceProduct
{
    public string Title { get; set; }
    public decimal PriceForItem { get; set; }

    public override string ToString()
    {
        return $"{Title} ({PriceForItem} per item)";
    }
}

public class FuelOrderItem
{
    public Fuel Fuel { get; set; }
    public decimal Amount { get; set; }

    public override string ToString()
    {
        return $"{Fuel.Brand}: {Amount} liters";
    }
}

public class PieceProductOrderItem
{
    public PieceProduct Product { get; set; }
    public int Amount { get; set; }

    public override string ToString()
    {
        return $"{Product.Title}: {Amount} items";
    }
}

public class DiscountCard
{
    public decimal FuelDiscountPercent { get; set; }
    public decimal FuelBonusPercent { get; set; }
    public decimal ProductBonusPercent { get; set; }

    public override string ToString()
    {

        return $"Fuel Discount: {FuelDiscountPercent}%, Fuel Bonus: {FuelBonusPercent}%, Product Bonus: {ProductBonusPercent}%";
    }
}

public class Order
{
    public List<FuelOrderItem> FuelItems { get; set; } = new();
    public List<PieceProductOrderItem> PieceItems { get; set; } = new();
    public DiscountCard DiscountCard { get; set; }

    public (decimal Total, decimal Bonuses) CalculateOrder()
    {
        decimal total = 0;
        decimal bonuses = 0;

        foreach (var item in FuelItems)
        {
            decimal fuelCost = item.Fuel.PriceForLiter * item.Amount;
            if (DiscountCard != null)
            {
                fuelCost *= (1 - DiscountCard.FuelDiscountPercent / 100);
                bonuses += fuelCost * (DiscountCard.FuelBonusPercent / 100);
            }
            total += fuelCost;
        }

        foreach (var item in PieceItems)
        {
            decimal productCost = item.Product.PriceForItem * item.Amount;
            total += productCost;
            if (DiscountCard != null)
                bonuses += productCost * (DiscountCard.ProductBonusPercent / 100);
        }

        return (total, bonuses);
    }

    public override string ToString()
    {
        var fuelDetails = string.Join(", ", FuelItems.Select(f => f.ToString()));
        var pieceDetails = string.Join(", ", PieceItems.Select(p => p.ToString()));
        var discountCardInfo = DiscountCard != null ? $", Discount Card: {DiscountCard}" : "";
        return $"Fuel Items: [{fuelDetails}],\nPiece Items: [{pieceDetails}]\n{discountCardInfo}";
    }
}

class Program
{
    static void Main()
    {
        var order = new Order
        {
            FuelItems = new List<FuelOrderItem>
            {
                new FuelOrderItem { Fuel = new Fuel { Brand = "95", PriceForLiter = 52 }, Amount = 30 },
                new FuelOrderItem { Fuel = new Fuel { Brand = "Diesel", PriceForLiter = 50 }, Amount = 50 }
            },
            PieceItems = new List<PieceProductOrderItem>
            {
                new PieceProductOrderItem { Product = new PieceProduct { Title = "Coca-cola", PriceForItem = 50 }, Amount = 2 },
                new PieceProductOrderItem { Product = new PieceProduct { Title = "HotDog", PriceForItem = 60 }, Amount = 1 }
            },
            DiscountCard = new DiscountCard
            {
                FuelDiscountPercent = 5,
                FuelBonusPercent = 2,
                ProductBonusPercent = 1
            }
        };

        var (total, bonuses) = order.CalculateOrder();
        Console.WriteLine($"Order Details:\n{order}");
        Console.WriteLine($"\nTotal cost: {total}, Bonuses: {bonuses}");


        var pieceItems = new List<PieceProductOrderItem>
        {
            new PieceProductOrderItem { Product = new PieceProduct { Title = "Coca-cola", PriceForItem = 50 }, Amount = 2 },
            new PieceProductOrderItem { Product = new PieceProduct { Title = "HotDog", PriceForItem = 60 }, Amount = 2 },
            new PieceProductOrderItem { Product = new PieceProduct { Title = "Chewing gum", PriceForItem = 20 }, Amount = 1 }
        };

        var sortedItems = pieceItems.OrderBy(item => item.Product.Title).ToList();

        Console.WriteLine("\nSorted products:");
        foreach (var item in sortedItems)
            Console.WriteLine(item);

    }
}