namespace Initializer
{
    using Core;
    using System;
    using System.Data.Entity;
    class Program
    {
        static void Main()
        {
            string menu = string.Empty;
            while (menu != "0")
            {
                Console.WriteLine("1. Инициализировать и заполнить БД\n0. Выход");
                menu = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
                switch (menu)
                {
                    case "1":
                        {
                            var initializer = new DbInitializer();
                            Database.SetInitializer(initializer);
                            using (var context = new SDContext())
                            {
                                initializer.InitializeDatabase(context);
                            }
                            Console.WriteLine("Объекты успешно сохранены");
                            break;
                        };
                    default:
                        break;
                };
            }
        }
    }
}