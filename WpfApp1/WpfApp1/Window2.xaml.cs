using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        int Id;
        Window1 window;
        public Window2(object id, Window1 window)
        {
            this.window = window;
            Id = (int)id;
            MessageBox.Show(Id.ToString());
            InitializeComponent();
            DataContext =  Entities.GetContext().Расписание.Where(p => p.Код_расписания == Id).First();

            CoupleComboBox.ItemsSource = Entities.GetContext().Пары.ToList();
            ParityComboBox.ItemsSource = Entities.GetContext().Четность_недели.ToList();
            GroupComboBox.ItemsSource = Entities.GetContext().Группы.ToList();
            AuditoriumComboBox.ItemsSource = Entities.GetContext().Аудитории.ToList();
            TypeOfOccupationComboBox.ItemsSource = Entities.GetContext().Виды_занятий.ToList();
            DayComboBox.ItemsSource = Entities.GetContext().Дни_недели.ToList();
            DisciplineComboBox.ItemsSource = Entities.GetContext().Дисциплина.ToList();
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Entities.GetContext().SaveChanges();
            window.UpdateData();
            this.Close();

        }
    }
}
