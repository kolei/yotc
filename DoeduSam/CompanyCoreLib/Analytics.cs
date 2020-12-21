using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCoreLib
{
    // вспомогательный класс, который поможет отсортировать даты по частоте использования
    class DateTimeWithCounter
    {
        public DateTime DateTimeProp;
        public int Counter = 0;

        public DateTimeWithCounter(DateTime date) {
            DateTimeProp = date;
            Counter = 1;
        }
    }

    public class Analytics
    {
        public List<DateTime> PopularMonths(List<DateTime> dates)
        {
            var DateTimeWithCounterList = new List<DateTimeWithCounter>();

            foreach (DateTime date in dates) {
                var DateMonthStart = new DateTime(date.Year, date.Month, 1, 0, 0, 0);

                var index = DateTimeWithCounterList.FindIndex(item => item.DateTimeProp == DateMonthStart);

                if (index == -1)
                {
                    // такой даты нет - добавляю
                    DateTimeWithCounterList.Add(new DateTimeWithCounter(DateMonthStart));
                }
                else {
                    // дата есть - увеличиваем счетчик
                    DateTimeWithCounterList[index].Counter++;
                }
            }

            return DateTimeWithCounterList
                .OrderByDescending(item => item.Counter)
                .ThenBy(item => item.DateTimeProp)
                .Select(item => item.DateTimeProp)
                .ToList();
        }
    }
}
