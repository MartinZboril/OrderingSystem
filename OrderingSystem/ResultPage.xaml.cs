using OrderingSystem.Model;
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

namespace OrderingSystem
{
    /// <summary>
    /// Interakční logika pro ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        Order Order = new Order();
        User User = new User();
        public ResultPage(Order order, User user)
        {
            InitializeComponent();
            Order = order;
            User = user;
            Message.Text = "Vaše objednávka byla přijata pod číslem " + Order.Number + ".";

            Name.Text = User.Name;
            Surname.Text = User.Surname;
            Email.Text = User.Email;
            Phone.Text = User.Phone.ToString();

            Number.Text = Order.Number.ToString();
            Price.Text = Order.Price.ToString();

            Town.Text = Order.Town;
            Street.Text = Order.Street;
            PostNumber.Text = Order.PostNumber.ToString();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new CatalogPage());
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new RegistrationPage());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new LoginPage());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new ProfilePage(User));
        }
    }
}
