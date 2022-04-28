using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Meeting 
    {
        public DateTime Start;
        public DateTime End;
        public DateTime Notification;

        public Meeting(DateTime start, DateTime end, DateTime notification)
        {
            if (end < start)
            {
                throw new Exception("Дата окончания не может быть раньше начала!!!");
            }
            if (notification > start)
            {
                throw new Exception("Дата уведомления не может быть позже начала встречи!!!");
            }
            Start = start;
            End = end;
            Notification = notification;
        }

        public override string ToString()
        {
                return $"Заплонирована встреча: {Start} - {End}";
        }
    }
}
