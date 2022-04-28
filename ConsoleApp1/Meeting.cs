namespace ConsoleApp1
{
    public class Meeting 
    {
        public DateTime Start;
        public DateTime End;
        public DateTime Notification;

        public Meeting(DateTime start, DateTime end, DateTime notification)
        {
            Start = start;
            End = end;
            Notification = notification;
        }
        public override string ToString()
        {
                return $"{Start.Day}.{Start.Month}.{Start.Year} {Start.TimeOfDay} - {End.TimeOfDay} напоминание в: {Notification.TimeOfDay}";
        }
    }
}