using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1.DataGrid
{
    class Schedule
    {
        private Window1 window;

        public Schedule(Window1 window)
        {
            this.window = window;
            var schedule = from расписание in Entities.GetContext().Расписание
                           join пара in Entities.GetContext().Пары on расписание.Пара equals пара.Код_пары
                           join четность_Недели in Entities.GetContext().Четность_недели on расписание.Четность equals четность_Недели.Код_четности
                           join группа in Entities.GetContext().Группы on расписание.Группа equals группа.Код_группы
                           join аудитория in Entities.GetContext().Аудитории on расписание.Аудитория equals аудитория.Код_аудитории
                           join вид_Занятия in Entities.GetContext().Виды_занятий on расписание.Вид_занятия equals вид_Занятия.Код_вида
                           join день_недели in Entities.GetContext().Дни_недели on расписание.День_недели equals день_недели.Код_дня_недели
                           join дисциплина in Entities.GetContext().Дисциплина on расписание.Дисциплина equals дисциплина.Код_дисциплины
                           select new
                           {
                               Код = расписание.Код_расписания,
                               CoupleNumber = пара.Номер_пары,
                               Parity = четность_Недели.Неделя,
                               Group = группа.Группа,
                               Auditorium = аудитория.Аудитория,
                               TypeOfOccupation = вид_Занятия.Вид_занятия,
                               Day = день_недели.День_недели,
                               Discipline = дисциплина.Дисциплина1
                           };

            window.DataGrid.ItemsSource = schedule.ToList();
            window.DataGrid.MouseDoubleClick += window.DataGrid_MouseDoubleClick;
        }
        public string[] GetCoupleTime(int id)
        {
            string[] coupleTime = new string[2];
            coupleTime[0] = Convert.ToString(Entities.GetContext().Расписание.Where(p => p.Пара == id).FirstOrDefault().Пары.Время_начала);
            coupleTime[1] = Convert.ToString(Entities.GetContext().Расписание.Where(p => p.Пара == id).FirstOrDefault().Пары.Время_окончания);
            return coupleTime;
        }
    }
}