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
    /// Interakční logika pro CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        dataService dataservice = new dataService();
        ObservableCollection<Goods> cart = new ObservableCollection<Goods>();
        User User = new User();

        public CartPage()
        {
            InitializeComponent();
        }

        public CartPage(ObservableCollection<Goods> CartGoodsList)
        {
            InitializeComponent();
            cart = CartGoodsList;
            listView.ItemsSource = cart;
            GetValueForShopCartInfo(cart);
        }

        public CartPage(ObservableCollection<Goods> CartGoodsList, User user)
        {
            InitializeComponent();
            Registration.Visibility = Visibility.Hidden;
            Login.Visibility = Visibility.Hidden;
            Profile.Visibility = Visibility.Visible;
            cart = CartGoodsList;
            listView.ItemsSource = cart;
            User = user;
            GetValueForShopCartInfo(cart);
        }

        public void GetValueForShopCartInfo(ObservableCollection<Goods> GoodsFromCart)
        {
            int TotalPrice = GetTotalPriceOfSelectedGoods(GoodsFromCart);
            PriceOFSelectedGoods.Text = TotalPrice.ToString() + " " + "Kč";
            int TotalPiece = GoodsFromCart.Count;
            PieceOFSelectedGoods.Text = TotalPiece.ToString() + " " + "Položek";
        }

        public int GetTotalPriceOfSelectedGoods(ObservableCollection<Goods> cartgoods)
        {
            int TotalPrice = 0;
            for (int i = 0; i < cartgoods.Count; i++)
            {
                TotalPrice += cartgoods[i].Price;
            }
            return TotalPrice;
        }

        private void PieceOfGoods_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (User.ID != 0)
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new OrderPage(User, cart));
            }
            else
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new OrderPage(cart));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
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
            ns.Navigate(new ProfilePage(User, cart));
        }

        private void DeleteFormCart_Button_Click(object sender, RoutedEventArgs e)
        {
            string item = (e.Source as Button).Tag.ToString();
            int ID = int.Parse(item);

            var itemToRemove = cart.SingleOrDefault(r => r.IDcart == ID);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
            }

            listView.ItemsSource = cart;
            GetValueForShopCartInfo(cart);
        }
    }
}
