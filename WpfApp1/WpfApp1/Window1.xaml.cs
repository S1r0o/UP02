using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Список_пользователей _user;
        private MainWindow window;
        private DataGrid.Schedule schedule;

        public Window1(Список_пользователей _user, MainWindow window)
        {
            InitializeComponent();
            this.window = window;
            this._user = _user;
            userName.Content = _user.Имя_пользователя;
            if (!String.IsNullOrEmpty(_user.Ссылка_на_фото))
            {
                try
                {
                    ImageBrush imageBrush = new ImageBrush
                    {
                        ImageSource = new BitmapImage(new Uri(System.IO.Path.GetFullPath(_user.Ссылка_на_фото)))
                    };
                    ImageUser.Fill = imageBrush;
                    window.Hide();
                    this.Show();
                    schedule = new DataGrid.Schedule(this);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            window.Show();
            window.Login.Text = "";
            window.Password.Password = "";
        }

        public void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid.CurrentCell.Column.Header.ToString() == "CoupleNumber")
            {
                var id = DataGrid.SelectedItem;
                List<string> sptl = id.ToString().Split(',').ToList();
                sptl[1] = sptl[1].Remove(0, "CoupleNumber = ".Length + 1);
                int d = Convert.ToInt32(sptl[1].ToString());
                MessageBox.Show($"Начало {schedule.GetCoupleTime(d)[0]} - Окончание {schedule.GetCoupleTime(d)[1]}");
            }
        }


        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            Window2 window2 = new Window2(button.Tag, this);
            window2.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.jfif;"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy(openFileDialog.FileName, System.IO.Path.GetFileName(openFileDialog.FileName), true);
                    _user.Ссылка_на_фото = System.IO.Path.GetFileName(openFileDialog.FileName);
                    Entities.GetContext().SaveChanges();
                    Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    try
                    {
                        ImageBrush imagebrush = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri(System.IO.Path.GetFullPath(_user.Ссылка_на_фото)))
                        };
                        ImageUser.Fill = imagebrush;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                catch (IOException exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }
        public void UpdateData()
        {
            Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
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
            this.DataGrid.ItemsSource = schedule.ToList();
            DataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Window3 window3 = new Window3
            {
                Owner = this
            };
            window3.Show();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteButton.IsEnabled = true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int i = (int)TypeDescriptor.GetProperties(DataGrid.SelectedItem)[0].GetValue(DataGrid.SelectedItem);
                Расписание расписание = Entities.GetContext().Расписание.Where(p => p.Код_расписания == i).First();
                Entities.GetContext().Расписание.Remove(расписание);
                Entities.GetContext().SaveChanges();
                UpdateData();
            }
        }
    }
}
