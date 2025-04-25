using System;
using System.Threading.Tasks;

namespace NatureSimulation
{
    // Делегат для подій часу доби
    public delegate void TimeEvent(string message);

    // Клас Сонце
    public class Sun
    {
        // Події дня: схід, ранок, полудень, надвечір'я, захід
        public event TimeEvent Sunrise;
        public event TimeEvent Morning;
        public event TimeEvent Noon;
        public event TimeEvent Evening;
        public event TimeEvent Sunset;

        // Метод, що імітує перебіг дня,думав над використанням Thread.Sleep
        // але при спробах написанні графічного варіанту (що в мене не вийшло)
        //блокувався потік і в мене не закривався exe програми і VS крашилась після повторного запуску і не виводилась правильно анімація
        //тому вирішив знайти альтернативу та використовувати асинхронні виклики

        public async Task StartDay()
        {
         // встановлення кольорів для різних фаз дня щоб краще візуально розрізняти
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Сонце сходить...");
            Sunrise?.Invoke("Схід сонця");
            await Task.Delay(2000); // Затримка 2 секунди
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Ранок...");
            Morning?.Invoke("Ранок");
            await Task.Delay(2000); // Затримка 2 секунди
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Полудень...");
            Noon?.Invoke("Полудень");
            await Task.Delay(2000); // Затримка 2 секунди
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Надвечір'я...");
            Evening?.Invoke("Надвечір'я");
            await Task.Delay(2000); // Затримка 2 секунди
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Сонце заходить...");
            Sunset?.Invoke("Захід сонця");
            await Task.Delay(2000); // Затримка 2 секунди
            Console.ResetColor();
        }
    }

    // Клас Місяць
    public class Moon
    {
        // Події ночі: схід місяця, захід місяця
        public event TimeEvent Moonrise;
        public event TimeEvent Moonset;

        // Метод, що імітує перебіг ночі
        public async Task StartNight()
        {
         // встановлення кольорів для різних фаз дня щоб краще візуально розрізняти
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Місяць сходить...");
            Moonrise?.Invoke("Схід місяця");
            await Task.Delay(5000); // Затримка 5 секунд
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Місяць заходить...");
            Moonset?.Invoke("Захід місяця");
            await Task.Delay(5000); // Затримка 5 секунд
            Console.ResetColor();
        }
    }

    // Базовий клас Квітка
    public abstract class Flower
    {
        public string Name { get; set; } // Назва квітки
        public int BloomingDays { get; set; } // Тривалість цвітіння (дні)
        public int DaysLeft { get; set; } // Кількість днів, що залишились до засихання
        public bool IsDried { get; private set; } // Чи засохла квітка

        // Конструктор базового класу
        protected Flower(string name, int bloomingDays)
        {
            Name = name;
            BloomingDays = bloomingDays;
            DaysLeft = bloomingDays;
            IsDried = false;
        }

        // Абстрактний метод для відкриття квітки
        public abstract void Open();

        // Абстрактний метод для закриття квітки
        public abstract void Close();

        // Перевірка стану квітки (чи закінчився цикл цвітіння)
        // тут теж думав щоб квіти зникали кожні 3 дні
        // але тоді протягом трьох днів б не виводились взаємодії
        // тому засихать деякі квітки а взаємодії відбувають з іншими

        public void CheckBlooming()
        {
            if (DaysLeft <= 0)
            {
                IsDried = true;
                Console.WriteLine($"{Name} засохла. 3-денний цикл закінчився.");
                DaysLeft = BloomingDays; // Почати новий цикл цвітіння
            }
        }

        // Зменшення лічильника днів до засихання
        public void PassDay()
        {
            DaysLeft--;
        }
    }

    // Денна квітка
    public class DayFlower : Flower
    {
        public DayFlower(string name) : base(name, 3) { }

        // Реалізація методу відкриття квітки
        public override void Open()
        {
            Console.WriteLine($"{Name} розкривається.");
        }

        // Реалізація методу закриття квітки
        public override void Close()
        {
            Console.WriteLine($"{Name} закривається.");
        }

        // Метод захисту від спеки
        public void ProtectFromHeat()
        {
            Console.WriteLine($"{Name} закриває листя від спеки.");
        }
    }

    // Вечірня квітка
    public class NightFlower : Flower
    {
        public NightFlower(string name) : base(name, 3) { }

        // Реалізація методу відкриття квітки
        public override void Open()
        {
            Console.WriteLine($"{Name} розкривається.");
        }

        // Реалізація методу закриття квітки
        public override void Close()
        {
            Console.WriteLine($"{Name} закривається.");
        }
    }

    // Клас Бджілка
    public class Bee
    {
        // Бджілка вилітає з вулика
        public void LeaveHive()
        {
            Console.WriteLine("Бджілка вилітає з вулика.");
        }

        // Бджілка збирає нектар з квітки
        public void CollectNectar(Flower flower)
        {
            Console.WriteLine($"Бджілка збирає нектар з {flower.Name}.");
        }

        // Бджілка повертається до вулика
        public void ReturnToHive()
        {
            Console.WriteLine("Бджілка повертається до вулика.");
        }
    }

    // Клас Нічний метелик
    public class Moth
    {
        // Нічний метелик відвідує квітку
        public void VisitFlower(Flower flower)
        {
            Console.WriteLine($"Нічний метелик відвідує {flower.Name}.");
        }
    }

    // Клас Дівчинка
    public class Girl
    {
        // Дівчинка милується квіткою
        public void EnjoyFlowers(Flower flower)
        {
            Console.WriteLine($"Дівчинка милується квіткою {flower.Name}.");
        }

        // Дівчинка робить селфі з квіткою
        public void TakeSelfieWithFlower(Flower flower)
        {
            Console.WriteLine($"Дівчинка робить селфі з {flower.Name}.");
        }
    }

    // Головний клас програми
    class NatureSimulation
    {
        static async Task Main(string[] args)
        {
            // Встановлення кодування для української мови через неправильне виведення букви і
          
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Оголошення масиву днів тижня
            string[] daysOfWeek = { "Понеділок", "Вівторок", "Середа", "Четвер", "Пʼятниця", "Субота", "Неділя" };
            int dayIndex = 0; // Індекс поточного дня тижня

            // Створення об'єктів
            var sun = new Sun(); // Сонце
            var moon = new Moon(); // Місяць
            var dayFlower = new DayFlower("Ромашка"); // Денна квітка
            var nightFlower = new NightFlower("Матіола"); // Вечірня квітка
            var bee = new Bee(); // Бджілка
            var moth = new Moth(); // Нічний метелик
            var girl = new Girl(); // Дівчинка

            // Підписка на події дня
            sun.Sunrise += (msg) =>
            {
                bee.LeaveHive(); // Бджілка вилітає з вулика
                dayFlower.Open(); // Денна квітка розкривається
            };

            sun.Morning += (msg) => bee.CollectNectar(dayFlower); // Бджілка збирає нектар з денної квітки
            sun.Noon += (msg) =>
            {
                dayFlower.ProtectFromHeat(); // Денна квітка захищається від спеки

                // Дівчинка взаємодіє з квітками в залежності від дня тижня
                string currentDay = daysOfWeek[dayIndex % 7];
                if (currentDay == "Субота" || currentDay == "Неділя")
                {
                    girl.EnjoyFlowers(dayFlower); // Дівчинка милується денними квітами
                    girl.TakeSelfieWithFlower(dayFlower); // Дівчинка робить селфі з денними квітами
                }
                else
                {
                    girl.EnjoyFlowers(nightFlower); // Дівчинка милується вечірніми квітами
                    girl.TakeSelfieWithFlower(nightFlower); // Дівчинка робить селфі з вечірніми квітами
                }
            };

            sun.Evening += (msg) =>
            {
                dayFlower.Close(); // Денна квітка закривається
                bee.ReturnToHive(); // Бджілка повертається до вулика
            };

            sun.Sunset += (msg) => nightFlower.Open(); // Вечірня квітка розкривається
            moon.Moonrise += (msg) => moth.VisitFlower(nightFlower); // Нічний метелик відвідує вечірню квітку
            moon.Moonset += (msg) => nightFlower.Close(); // Вечірня квітка закривається

            // Імітація перебігу часу (14 днів)
            for (int day = 1; day <= 14; day++)
            {
                string currentDay = daysOfWeek[dayIndex % 7]; // Поточний день тижня
                Console.WriteLine($"\n--- {currentDay} ---");

                // Перевірка цвітіння квіток
                dayFlower.CheckBlooming();
                nightFlower.CheckBlooming();

                // День і ніч
                await sun.StartDay();
                await moon.StartNight();

                // Зменшення лічильника днів до засихання
                dayFlower.PassDay();
                nightFlower.PassDay();

                dayIndex++; // Переходимо до наступного дня тижня
            }
        }
    }
}