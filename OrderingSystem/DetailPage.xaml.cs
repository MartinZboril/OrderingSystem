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
    /// Interakční logika pro DetailPage.xaml
    /// </summary>
    public partial class DetailPage : Page
    {
        dataService dataservice = new dataService();
        ObservableCollection<Goods> goods = new ObservableCollection<Goods>();
        ObservableCollection<Goods> cart = new ObservableCollection<Goods>();
        User User = new User();

        public DetailPage(Goods goods, ObservableCollection<Goods> CartGoodsList)
        {
            InitializeComponent();
            Name.Text = goods.Name;
            ImageOfAlbum.Source = new BitmapImage(new Uri($"{goods.Image}", UriKind.Absolute));
            YearOfRealising.Text = goods.YearOfRealising.ToString();
            Category.Text = goods.Category;
            Type.Text = goods.Type;
            Price.Text = goods.Price.ToString() + " Kč";
            Description.Text = goods.Content;
            Buy_Button.Tag = goods.ID;
            GetCategories();
            cart = CartGoodsList;
            GetValueForShopCartInfo(cart);
        }

        public DetailPage(Goods goods, User user, ObservableCollection<Goods> CartGoodsList)
        {
            InitializeComponent();
            User = user;
            RegistrationButton.Visibility = Visibility.Hidden;
            LoginButton.Visibility = Visibility.Hidden;
            Profile.Visibility = Visibility.Visible;

            Name.Text = goods.Name;          
            ImageOfAlbum.Source = new BitmapImage(new Uri($"{goods.Image}", UriKind.Absolute));
            YearOfRealising.Text = goods.YearOfRealising.ToString();
            Category.Text = goods.Category;
            Type.Text = goods.Type;
            Price.Text = goods.Price.ToString() + " Kč";
            Description.Text = goods.Content;
            Buy_Button.Tag = goods.ID;
            GetCategories();
            cart = CartGoodsList;
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

        public async Task GetCategories()
        {
            goods = await dataservice.GetGoodsData();

            List<string> categories = new List<string>();

            for (int i = 0; i < goods.Count; i++)
            {
                if (categories.Contains(goods[i].Category))
                {

                }
                else
                {
                    categories.Add(goods[i].Category);
                }
            }

            ObservableCollection<Goods> goodsCategories = new ObservableCollection<Goods>();

            for (int x = 0; x < categories.Count; x++)
            {
                Goods goodsToList = new Goods();
                goodsToList.Category = categories[x];
                goodsCategories.Add(goodsToList);
            }

            ListViewOfCategories.ItemsSource = goodsCategories;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (User.ID == 0)
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new CatalogPage(cart));
            }
            else
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new CatalogPage(User, cart));
            }
        }

        private void ListViewOfCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewOfCategories.SelectedItem != null)
            {
                var item = ListViewOfCategories.SelectedItem as Goods;

                if (User.ID == 0)
                {
                    NavigationService ns = NavigationService.GetNavigationService(this);
                    ns.Navigate(new CatalogPage(item, cart));
                }
                else
                {
                    NavigationService ns = NavigationService.GetNavigationService(this);
                    ns.Navigate(new CatalogPage(User, cart));
                }
            }
        }

        private async void Buy_Button_Click(object sender, RoutedEventArgs e)
        {
            string item = (e.Source as Button).Tag.ToString();
            ObservableCollection<Goods> selectedGoods = new ObservableCollection<Goods>();
            selectedGoods = await dataservice.GetGoodsByIDData(item);

            if (cart.Count > 0)
            {
                var maxIDcart = cart.Max(m => m.IDcart);
                selectedGoods[0].IDcart = maxIDcart + 1;
            }
            else
            {
                selectedGoods[0].IDcart = 1;
            }

            cart.Add(selectedGoods[0]);
            PieceOFSelectedGoods.Text = cart.Count.ToString();

            if (User.ID != 0)
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new CartPage(cart, User));
            }
            else
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new CartPage(cart));
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new LoginPage());
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new RegistrationPage());
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new ProfilePage(User, cart));
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            if (User.ID == 0)
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new CartPage(cart, User));
            }
            else
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new CartPage(cart));
            }
        }
    }
}
