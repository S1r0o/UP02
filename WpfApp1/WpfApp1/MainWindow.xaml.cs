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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Log_in_Click(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(Login.Text))
                if(!String.IsNullOrEmpty(Password.Password))
                {
                    IQueryable<Список_пользователей> userList = Entities.GetContext().Список_пользователей.Where(p => p.Логин == Login.Text && p.Пароль == Password.Password);
                    if (userList.Count() == 1)
                    {
                        MessageBox.Show($"Hello, {userList.First().Имя_пользователя} приступим к работе", "Удачная авторизация", MessageBoxButton.OK);
                        Window1 window = new Window1(userList.First(), this)
                        {
                            Owner = this
                        };
                    }
                    else MessageBox.Show("Без пароля уходи!");
                }
        }
    }
}
