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
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
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
            StringBuilder errors = new StringBuilder();
            if (CoupleComboBox.SelectedItem == null)
                errors.AppendLine("Укажите номер пары");
            if (ParityComboBox.SelectedItem == null)
                errors.AppendLine("Укажите четность недели");
            if (GroupComboBox.SelectedItem == null)
                errors.AppendLine("Укажите группу");
            if (AuditoriumComboBox.SelectedItem == null)
                errors.AppendLine("Укажите номер аудитории");
            if (TypeOfOccupationComboBox.SelectedItem == null)
                errors.AppendLine("Укажите вид занятия");
            if (DayComboBox.SelectedItem == null)
                errors.AppendLine("Укажите день недели");
            if (DisciplineComboBox.SelectedItem == null)
                errors.AppendLine("Укажите дисциплину");
            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
                
            }
            Entities.GetContext().Расписание.Add(new Расписание()
            {
                Пары = CoupleComboBox.SelectedItem as Пары,
                Четность_недели = ParityComboBox.SelectedItem as Четность_недели,
                Группы = GroupComboBox.SelectedItem as Группы,
                Аудитории = AuditoriumComboBox.SelectedItem as Аудитории,
                Виды_занятий = TypeOfOccupationComboBox.SelectedItem as Виды_занятий,
                Дни_недели = DayComboBox.SelectedItem as Дни_недели,
                Дисциплина1 = DisciplineComboBox.SelectedItem as Дисциплина,
            });
            Entities.GetContext().SaveChanges();
            ((Window1)this.Owner).UpdateData();
            MessageBox.Show("Данные добавлены");
            this.Close();

        }
    }
}
