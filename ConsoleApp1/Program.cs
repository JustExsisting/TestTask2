using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    public class Program
    {
        static List<Meeting> list = new();
        static int cnt = 0; //счётчик запущенных тасков
        static Action<Int32, ParallelLoopState> Notification()
        {
            DateTime dt;
            for (int i = 0; ; i++) //бесконечный цикл с перебором всего списка
            {
                dt = DateTime.Now;

                if (i == list.Count) //если i выходит за пределы спика, тогда i = 0 и начать цикл сначала
                {
                    i = 0;
                    continue;
                }

                if (dt == list[i].Notification)
                {
                    Console.WriteLine($"Заплонирована встреча:\n{list[i].Start} - {list[i].End}");
                }
            }
        }

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);
        static void PrintMenu()
        {
            Console.WriteLine("Добавить встречу - 1\nИзменить встречу - 2\nУдалить встречу - 3\nВыход - 0");
        }

        static void Add()
        {
            bool exit = false; //флаг для полного выхода из функции
            Exception exception = null;
            while (!exit) //цикл для добавления встречи, цикл нужен, чтобы не выкидывало в меню при появлении ошибки.
            {
                if (exception == null)
                {
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Нажмите Enter, чтобы продолжить");
                    Console.ReadLine();
                    Console.Clear();
                    exception = null;
                }
                Console.WriteLine("Добавление новой встречи\n\nПродолжить - Enter\nВыхода - 0, затем Enter");
                string command = Console.ReadLine();

                if (command == "0") // если пользователь хочет выйти из Добавления встречи
                {
                    exit = true;
                    break;
                }
                DateTime start, end, notification;
                try
                {
                    Console.WriteLine("Пример ввода даты: дд.ММ.гггг чч:мм");
                    Console.Write("Введите начало встречи: ");
                    start = DateTime.Parse(Console.ReadLine());
                    Console.Write("Введите конец встречи: ");
                    end = DateTime.Parse(Console.ReadLine());
                    Console.Write("Введите во сколько напомнить о встрече: ");
                    notification = DateTime.Parse(Console.ReadLine());

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (start < list[i].End || end > list[i].Start) //проверка на наличие встреч в ведённое время
                        {
                            throw new Exception("На это время уже есть встреча");
                        }
                    }
                    list.Add(new Meeting(start, end, notification));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    if (ex != null)
                    {
                        exception = ex;
                    }
                }
                if (exception == null) //выход из цикла while если ошибок нет
                {
                    break;
                }
            }

            if (!exit)
            {
                try
                {
                    if (cnt == 0) // если есть активный таск, то новый не запустится
                    {
                        MessageBox((IntPtr)0, "Your Message", "My Message Box", 0);
                        //Parallel.For(0, list.Count(), Notification());
                        //task.Start();
                        cnt++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        static void GetAll()
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1} : {list[i].Start} - {list[i].End} Напоминание в : {list[i].Notification}");
            }
        }

        static void Edit()
        {
            Console.Clear();
            bool exit = false;
            while (!exit)
            {
                GetAll();
                Console.WriteLine("Выберете номер встречи которой хотите изменить:");
                try
                {
                    int index = int.Parse(Console.ReadLine()) - 1;
                    if (index >= list.Count)
                    {
                        throw new Exception("Введите коректно номер встречи");
                    }

                    DateTime start = list[index].Start;
                    DateTime end = list[index].End;
                    DateTime notification = list[index].Notification;

                    Console.WriteLine("Пример ввода даты: дд.ММ.гггг чч:мм");
                    Console.WriteLine("Если что-то менять не нужно - оставте пустую строку");

                    Console.Write("Введите начало встречи: ");
                    start = DateTime.Parse(Console.ReadLine());
                    Console.Write("Введите конец встречи: ");
                    end = DateTime.Parse(Console.ReadLine());
                    Console.Write("Введите во сколько напомнить о встрече: ");
                    notification = DateTime.Parse(Console.ReadLine());
                    if (start != null)
                    {
                        list[index].Start = start;
                    }
                    if (end != null)
                    {
                        list[index].End = end;
                    }
                    if (notification != null)
                    {
                        list[index].Notification = notification;
                    }
                }
                catch (Exception)
                {
                    exit = true;
                    break;
                }
            }
        }

        static void Remove()
        {
            Console.Clear();
            GetAll();
            Console.Write("Введите номер встречи которую необходимо удалить: ");
            int index = int.Parse(Console.ReadLine()) - 1;
        }

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                PrintMenu();
                string command = Console.ReadLine();
                switch (command)
                {
                    case "1":
                        Add();
                        break;
                    case "2":
                        Edit();
                        break;
                    case "3":
                        Remove();
                        break;
                    case "0":
                        exit = true;
                        break;
                }
            }
        }
    }
}