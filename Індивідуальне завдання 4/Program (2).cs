using System;
using System.Collections.Generic;
using System.Globalization;

namespace HypermarketOrder
{
    
    interface ILocalization
    {
        string FormatPrice(decimal price);
        string FormatDate(DateTime date);
    }

    
    class UkrainianLocalization : ILocalization
    {
        public string FormatPrice(decimal price) =>
            price.ToString("0.00").Replace('.', ',');

        public string FormatDate(DateTime date) =>
            date.ToString("dd.MM.yyyy");
    }
    class FrenchLocalization : ILocalization
    {
        public string FormatPrice(decimal price)
        {
            string integerPart = ((int)price).ToString("N0", CultureInfo.InvariantCulture);
            string fractionalPart = (price % 1).ToString("F2").Substring(1);
            return $"{integerPart}{fractionalPart}".Replace(',', ' ').Replace('.', ',');
        }

        public string FormatDate(DateTime date)
        {
          
            return date.ToString("dd/MM/yyyy");
        }
    }
    class EnglishLocalization : ILocalization
    {
        public string FormatPrice(decimal price)
        {
            string integerPart = FormatIntegerPart((int)price);
            string fractionalPart = ((price - Math.Truncate(price)).ToString("F2"))[2..];
            return $"{integerPart}.{fractionalPart}";
        }

        public string FormatDate(DateTime date) =>
            date.ToString("yyyy'/'MM'/'dd");

        private string FormatIntegerPart(int number)
        {
            var parts = new List<string>();
            string numStr = number.ToString();
            for (int i = numStr.Length; i > 0; i -= 3)
            {
                int start = Math.Max(0, i - 3);
                parts.Add(numStr[start..i]);
            }
            parts.Reverse();
            return string.Join(",", parts);
        }
    }

    abstract class Product
    {
        public string Title { get; }
        public decimal Price { get; }
        protected ILocalization Localization { get; }

        public Product(string title, decimal price, ILocalization localization)
        {
            Title = title;
            Price = price;
            Localization = localization;
        }

        public abstract string GetLocalizedInfo();
    }

 
    class Food : Product
    {
        private DateTime ExpirationDate { get; }

        public Food(string title, decimal price, DateTime expirationDate, ILocalization localization)
            : base(title, price, localization) =>
            ExpirationDate = expirationDate;

        public override string GetLocalizedInfo() =>
            $"{Title}\t{Localization.FormatPrice(Price)}\t{Localization.FormatDate(ExpirationDate)}\t-";
    }

   
    class HouseholdAppliance : Product
    {
        public int WarrantyPeriodInMonths { get; }

        public HouseholdAppliance(string title, decimal price, int warranty, ILocalization localization)
            : base(title, price, localization) =>
            WarrantyPeriodInMonths = warranty;

        public override string GetLocalizedInfo() =>
            $"{Title}\t{Localization.FormatPrice(Price)}\t-\t{WarrantyPeriodInMonths}";
    }

   
    class Program
    {
        static void Main()
        {
            ILocalization localization = GetLocalization();
            var products = new List<Product>
            {
                new Food("bread", 20.00m, new DateTime(2021, 4, 29), localization),
                new HouseholdAppliance("blender", 5000.00m, 12, localization),
                new Food("cheese", 150.50m, new DateTime(2021, 6, 15), localization),
                new HouseholdAppliance("microwave", 7000.00m, 24, localization)
            };

            PrintTable(products);
        }

        static ILocalization GetLocalization()
        {
            Console.WriteLine("Enter localization (EN, UA, FR):");
            string input = Console.ReadLine().Trim().ToUpper();

            return input switch
            {
                "UA" => new UkrainianLocalization(),
                "EN" => new EnglishLocalization(),
                "FR" => new FrenchLocalization(),
                _ => new EnglishLocalization()
            };
        }

        static void PrintTable(List<Product> products)
        {
            var headers = new[] { "title", "price", "expirationDate", "warrantyPeriodInMonths" };
            var rows = new List<string[]> { headers };
            rows.AddRange(products.ConvertAll(p => p.GetLocalizedInfo().Split('\t')));

            int[] maxWidths = new int[4];
            foreach (var row in rows)
                for (int i = 0; i < 4; i++)
                    maxWidths[i] = Math.Max(maxWidths[i], row[i].Length);

            foreach (var row in rows)
            {
                for (int i = 0; i < 4; i++)
                    Console.Write(row[i].PadRight(maxWidths[i] + 2));
                Console.WriteLine();
            }
        }
    }
}