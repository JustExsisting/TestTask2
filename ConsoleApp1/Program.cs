namespace ConsoleApp1
{
    public class Program
    {
        static List<Meeting> list = new();
        //static int cnt = 0; //счётчик запущенных уведомлений
        private static Meeting meeting;

        //static async void Notification()
        //{
        //    DateTime dt = DateTime.Now;
        //    await Task.Run(() =>
        //    {
        //        for (int i = 0; ; i++) //бесконечный цикл с перебором всего списка
        //        {
        //            dt = DateTime.Now;
        //            if (i == list.Count) //если i выходит за пределы спика, тогда i = 0 и начать цикл сначала
        //            {
        //                i = 0;
        //                continue;
        //            }
        //            if (dt == list[i].Notification)
        //                for (int j = 0; j < 1;)
        //                    if (Console.KeyAvailable == false)
        //                    {
        //                        Console.WriteLine($"Заплонирована встреча:\n{list[i].Start} - {list[i].End}");
        //                        j++;
        //                    }
        //        }
        //    });
        //}

        static void PrintMenu()
        {
            Console.WriteLine("Добавить встречу - 1\nИзменить встречу - 2\nУдалить встречу - 3\nВыход - 0");
        }
        static Exception Chek(DateTime start, DateTime end, DateTime notification)//проверка новой встречи
        {
            if (end < start)
                throw new Exception("Дата окончания не может быть раньше начала!!!");
            if (notification > start)
                throw new Exception("Дата уведомления не может быть позже начала встречи!!!");
            for (int i = 0; i < list.Count; i++)
            {
                int res1 = DateTime.Compare(start.Date, list[i].Start.Date); //проверка на совпадение дней
                int res2 = DateTime.Compare(end.Date, list[i].End.Date);
                if (start.Date == end.Date)
                {
                    if (res1 == 0 && res2 == 0) //если уже есть встречи в этот день
                    {
                        if (start < list[i].Start) //начало новой встречи раньше уже имеющейся
                        {
                            if (end > list[i].Start && end < list[i].End) //конец встречи не должен находится во временном промежутке другой встречи
                                throw new Exception("Конец встречи не должен находится во временном промежутке другой встречи!!!");
                            if (end > list[i].End) // встречи не должны пересикаться
                                throw new Exception("Новая встреча не должна пересекаться с другой!!!");
                        }
                        else //начало новой встречи позже уже имеющейся
                        {
                            if (end > list[i].Start && end < list[i].End) //конец встречи не должен находится во временном промежутке другой встречи
                                throw new Exception("Конец встречи не должен находится во временном промежутке другой встречи!!!");
                            if (start < list[i].End) //начало новой встречи находится в промежутке уже имеющейся встречи
                                throw new Exception("Начало новой встречи находится в промежутке уже имеющейся встречи!!!");
                        }
                    }
                }
                else
                    throw new Exception("Начало и конец встречи должны быть назначены в один день!!!");
            }
            if (start.Date != end.Date)
                throw new Exception("Начало и конец встречи должны быть назначены в один день!!!"); //рабочий день кончается раньше, чем сутки
            return null;
        }

        static void Add()
        {
            bool exit = false; //флаг для полного выхода из функции
            Exception exception = null;
            while (!exit) //цикл для добавления встречи, цикл нужен, чтобы не выкидывало в меню при появлении ошибки.
            {
                if (exception == null) //очишение консоли при первом входе
                    Console.Clear();
                else //уведомить об ошибке, затем очистить консоль
                {
                    Console.WriteLine("Нажмите Enter, чтобы продолжить\n");
                    Console.ReadLine();
                    Console.Clear();
                    exception = null;
                }
                Console.WriteLine("Вы уверены, что хотите добавить новую встречу?\nЧто угодно - продолжить\nEscape - выход");
                ConsoleKeyInfo command = Console.ReadKey();

                if (command.Key == ConsoleKey.Escape) // если пользователь хочет выйти из Добавления встречи
                {
                    exit = true;
                    break;
                }
                DateTime start, end, notification;
                try
                {
                    Example();
                    Console.Write("Введите начало встречи: ");
                    start = DateTime.Parse(Console.ReadLine());
                    Console.Write("Введите конец встречи: ");
                    end = DateTime.Parse(Console.ReadLine());
                    Console.Write("Введите во сколько напомнить о встрече: ");
                    notification = DateTime.Parse(Console.ReadLine());

                    Chek(start, end, notification);

                    list.Add(new Meeting(start, end, notification));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    if (ex != null)
                        exception = ex;
                }
                if (exception == null) //выход из цикла while если ошибок нет
                    break;
            }
            //if (!exit)
            //{
            //    try
            //    {
            //        if (cnt == 0) // если уведомелния запущены, то новые не запустятся
            //        {
            //            Notification();
            //            cnt++;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex);
            //    }
            //}
        }

        static void GetAll() //вывод всех встреч
        {
            Console.Clear();
            list = list.OrderBy(d => d.Start).ToList();//сортировка по началу встречи для наглядного отображения пользователю
            for (int i = 0; i < list.Count; i++)
                Console.WriteLine($"{i + 1} : {list[i]}"); //нумерация для пользователя начинается с 1
        }

        static void Edit() //изменение встречи
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Изменение встречи\n\nПродолжить - Что угодно\nВыхода - Escape");
                ConsoleKeyInfo cmd = Console.ReadKey();

                if (cmd.Key == ConsoleKey.Escape) // если пользователь хочет выйти из Изменения встречи
                {
                    exit = true;
                    break;
                }
                try
                {
                    GetAll();
                    Console.WriteLine("Вы уверены, что хотите что-то изменить?\n\nЧто угодно - продолжить\nEscape - выход");
                    cmd = Console.ReadKey();
                    if (cmd.Key == ConsoleKey.Escape) // если пользователь хочет выйти из Изменения встречи
                    {
                        exit = true;
                        break;
                    }
                    Console.WriteLine("\nВведите номер встречи, которую хотите изменить: ");
                    int index = SetIndex();

                    DateTime tmpStart, tmpEnd, tmpNotification;
                    meeting = new Meeting(list[index].Start, list[index].End, list[index].Notification);//создание резервной копии встречи

                    Console.WriteLine("\nУкажите что хотите изменить:\n1 - начало встречи\n2 - конец встречи\n3 - напоминание о встрече\n0 - выход");
                    string command = Console.ReadLine();

                    switch (command)
                    {
                        case "1":
                            Example();
                            Console.Write("Введите начало встречи: ");
                            tmpStart = DateTime.Parse(Console.ReadLine());
                            Console.Write("Введите во сколько напомнить о встрече: ");
                            tmpNotification = DateTime.Parse(Console.ReadLine());
                            list.RemoveAt(index);
                            Chek(tmpStart, meeting.End, tmpNotification);//если проверка не пройдена, тогда встреча не будет добавлена
                            list.Add(new Meeting(tmpStart, meeting.End, tmpNotification));//добавляем изменённую встречу
                            break;
                        case "2":
                            Example();
                            Console.Write("Введите конец встречи: ");
                            tmpEnd = DateTime.Parse(Console.ReadLine());
                            list.RemoveAt(index);
                            Chek(meeting.Start, tmpEnd, meeting.Notification);//если проверка не пройдена, тогда встреча не будет добавлена
                            list.Add(new Meeting(meeting.Start, tmpEnd, meeting.Notification));//добавляем изменённую встречу
                            break;
                        case "3":
                            Example();
                            Console.Write("Введите во сколько напомнить о встрече: ");
                            tmpNotification = DateTime.Parse(Console.ReadLine());
                            list.RemoveAt(index);
                            Chek(meeting.Start, meeting.End, tmpNotification);//если проверка не пройдена, тогда встреча не будет добавлена
                            list.Add(new Meeting(meeting.Start, meeting.End, tmpNotification));//добавляем изменённую встречу
                            break;
                        case "0":
                            break;
                    }
                }
                catch (Exception ex)
                {
                    list.Add(meeting);//если проверка не пройдена, тогда резервная копия помещается в список
                    Console.WriteLine(ex);
                    Console.WriteLine("Нажмите Enter, чтобы продолжить\n");
                    Console.ReadLine();
                }
            }
        }

        static void Remove()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Удаление встречи\n\nПродолжить - Что угодно\nВыхода - Escape");
                ConsoleKeyInfo cmd = Console.ReadKey();
                if (cmd.Key == ConsoleKey.Escape) // если пользователь хочет выйти из Удаления встречи
                {
                    exit = true;
                    break;
                }
                GetAll();
                Console.WriteLine("Вы уверены, что хотите что-то удалить?\n\nЧто угодно - продолжить\nEscape - выход");
                cmd = Console.ReadKey();
                if (cmd.Key == ConsoleKey.Escape) // если пользователь хочет выйти из Изменения встречи
                {
                    exit = true;
                    break;
                }
                Console.Write("Введите номер встречи которую необходимо удалить: ");
                try
                {
                    int index = SetIndex();
                    list.RemoveAt(index);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                    Console.WriteLine("Нажмите Enter, чтобы продолжить\n"); ;
                    Console.ReadLine();
                }
            }
            
        }
        static void Example()
        {
            Console.WriteLine("\nПример ввода даты: дд.ММ.гггг чч:мм");
        }
        static int SetIndex()
        {
            int index = int.Parse(Console.ReadLine()) - 1; //приведение индекса в рабочий вариант
            if (index >= list.Count || index < 0)
                throw new Exception("Введите коректно номер встречи");
            return index;
        }

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit) // меню выбора действия
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