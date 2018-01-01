using OrderingSystem.DataService;
using OrderingSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interakční logika pro OrderViewPage.xaml
    /// </summary>
    public partial class OrderViewPage : Page
    {
        dataService dataservice = new dataService();
        Order Order = new Order();
        User User = new User();
        ObservableCollection<Goods> cart = new ObservableCollection<Goods>();
        public OrderViewPage(User user, ObservableCollection<Goods> CartGoodsList)
        {
            InitializeComponent();
            User = user;
            cart = CartGoodsList;
        }

        public OrderViewPage(ObservableCollection<Goods> CartGoodsList)
        {
            InitializeComponent();
            cart = CartGoodsList;
        }

        private async void SearchOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int searchingOrderNumber = 0;
            if (SearchWordText.Text.Length != 0 && int.TryParse(SearchWordText.Text, out searchingOrderNumber))
            {
                ObservableCollection<Order> orders = new ObservableCollection<Order>();
                orders = await dataservice.GetOrdersData();

                for (int i = 0; i < orders.Count; i++)
                {
                    if (orders[i].Number == searchingOrderNumber && orders[i].Deleted == 0)
                    {
                        Order = orders[i];
                    }
                }

                ObservableCollection<User> users = new ObservableCollection<User>();
                users = await dataservice.GetUserData();
                User selectedUser = new User();

                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].ID == Order.UserID)
                    {
                        selectedUser = users[i];
                    }
                }
                
                if (Order.ID != 0 && selectedUser.ID != 0)
                {

                    Name.Text = selectedUser.Name;
                    Surname.Text = selectedUser.Surname;
                    Email.Text = selectedUser.Email;
                    Phone.Text = selectedUser.Phone.ToString();

                    Number.Text = Order.Number.ToString();
                    Price.Text = Order.Price.ToString();

                    Town.Text = Order.Town;
                    Street.Text = Order.Street;
                    PostNumber.Text = Order.PostNumber.ToString();
                    FailDisplay.Text = "";
                }
                else
                {
                    Name.Text = "";
                    Surname.Text = "";
                    Email.Text = "";
                    Phone.Text = "";

                    Number.Text = "";
                    Price.Text = "";

                    Town.Text = "";
                    Street.Text = "";
                    PostNumber.Text = "";
                    FailDisplay.Text = "Objednávka neexistuje!";
                }
            }
            else
            {
                Name.Text = "";
                Surname.Text = "";
                Email.Text = "";
                Phone.Text = "";

                Number.Text = "";
                Price.Text = "";

                Town.Text = "";
                Street.Text = "";
                PostNumber.Text = "";
                FailDisplay.Text = "Není vyplněno číslo objednávky nebo objednávka neexistuje!";
            }
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (User.ID != 0)
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new CatalogPage(User, cart));
            }
            else
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new CatalogPage(cart));
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new LoginPage());
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new RegistrationPage());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new ProfilePage(User, cart));
        }
    }
}
