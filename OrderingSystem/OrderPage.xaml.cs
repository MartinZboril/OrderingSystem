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
    /// Interakční logika pro OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        dataService dataservice = new dataService();
        ObservableCollection<Goods> cart = new ObservableCollection<Goods>();
        User User = new User();

        public OrderPage(ObservableCollection<Goods> CartGoodsList)
        {
            InitializeComponent();
            cart = CartGoodsList;
            GetValueForShopCartInfo(cart);
        }

        public OrderPage(User user, ObservableCollection<Goods> CartGoodsList)
        {
            InitializeComponent();
            User = user;
            Name.Text = User.Name;
            Name.IsReadOnly = true;
            Surname.Text = User.Surname;
            Surname.IsReadOnly = true;
            Email.Text = User.Email;
            Email.IsReadOnly = true;
            Phone.Text = User.Phone.ToString();
            Phone.IsReadOnly = true;
            cart = CartGoodsList;
            GetValueForShopCartInfo(cart);
            Registration.Visibility = Visibility.Hidden;
            Login.Visibility = Visibility.Hidden;
            Profile.Visibility = Visibility.Visible;
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
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

        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            int userID = 0;

            if (User.ID != 0)
            {
                userID = User.ID;


                if (!String.IsNullOrEmpty(Name.Text) && !String.IsNullOrEmpty(Surname.Text) && !String.IsNullOrEmpty(Email.Text) && !String.IsNullOrEmpty(Phone.Text) && !String.IsNullOrEmpty(Town.Text) && !String.IsNullOrEmpty(Street.Text) && !String.IsNullOrEmpty(PostNumber.Text))
                {
                    if (Email.Text.Contains("@") && Email.Text.Contains("."))
                    {
                        int phone = 0;
                        if (int.TryParse(Phone.Text, out phone))
                        {
                            int postNumber = 0;
                            if (int.TryParse(PostNumber.Text, out postNumber))
                            {                               
                                int orderPrice = 0;

                                for (int i = 0; i < cart.Count; i++)
                                {
                                    orderPrice += cart[i].Price;
                                }

                                Random r = new Random();
                                int rnd = r.Next();

                                Order order = new Order();
                                order.UserID = userID;
                                order.Number = rnd;
                                order.Price = orderPrice;
                                order.Town = Town.Text;
                                order.Street = Street.Text;
                                order.PostNumber = PostNumber.Text;
                                order.Deleted = 0;

                                ObservableCollection<Order> orders = new ObservableCollection<Order>();
                                orders = await dataservice.PostOrdersAsync(order);

                                PriceOFSelectedGoods.Text = orders[0].Number.ToString();

                                for (int i = 0; i < cart.Count; i++)
                                {
                                    GoodsOrder goodsOrder = new GoodsOrder();
                                    goodsOrder.UserID = userID;
                                    goodsOrder.GoodsID = cart[i].ID;
                                    goodsOrder.OrderID = orders[0].ID;

                                    await dataservice.PostGoodsOrdersAsync(goodsOrder);
                                }

                                NavigationService ns = NavigationService.GetNavigationService(this);
                                ns.Navigate(new ResultPage(order, User));

                            }
                            else
                            {
                                FailsDisplay.Text = "Popisné číslo není z číslic!";
                            }
                            
                        }
                        else
                        {
                            FailsDisplay.Text = "Telefon není z číslic!";
                        }
                    }
                    else
                    {
                        FailsDisplay.Text = "Nesprávný tvar emailu!";
                    }
                }
                else
                {
                    FailsDisplay.Text = "Není vše vyplněno!";
                }
            }
            else
            {
                ObservableCollection<User> users = new ObservableCollection<User>();
                users = await dataservice.GetUserData();

                if (!String.IsNullOrEmpty(Name.Text) && !String.IsNullOrEmpty(Surname.Text) && !String.IsNullOrEmpty(Email.Text) && !String.IsNullOrEmpty(Phone.Text) && !String.IsNullOrEmpty(Town.Text) && !String.IsNullOrEmpty(Street.Text) && !String.IsNullOrEmpty(PostNumber.Text))
                {
                    if (Email.Text.Contains("@") && Email.Text.Contains("."))
                    {
                        int phone = 0;
                        if (int.TryParse(Phone.Text, out phone))
                        {
                            bool contains = users.Any(p => p.Email == Email.Text);

                            if (contains)
                            {
                                FailsDisplay.Text = "Email už je zabraný!";
                            }
                            else
                            {
                                int postNumber = 0;
                                if (int.TryParse(PostNumber.Text, out postNumber))
                                {
                                    User user = new User();
                                    user.Name = Name.Text;
                                    user.Surname = Surname.Text;
                                    user.Email = Email.Text;
                                    user.Phone = int.Parse(Phone.Text);

                                    ObservableCollection<User> selectedUser = new ObservableCollection<User>();
                                    selectedUser = await dataservice.PostUserDataAsync(user);
                                    User = selectedUser[0];
                                    userID = selectedUser[0].ID;

                                    int orderPrice = 0;

                                    for (int i = 0; i < cart.Count; i++)
                                    {
                                        orderPrice += cart[i].Price;
                                    }

                                    Random r = new Random();
                                    int rnd = r.Next();

                                    Order order = new Order();
                                    order.UserID = userID;
                                    order.Number = rnd;
                                    order.Price = orderPrice;
                                    order.Town = Town.Text;
                                    order.Street = Street.Text;
                                    order.PostNumber = PostNumber.Text;
                                    order.Deleted = 0;

                                    ObservableCollection<Order> orders = new ObservableCollection<Order>();
                                    orders = await dataservice.PostOrdersAsync(order);

                                    PriceOFSelectedGoods.Text = orders[0].Number.ToString();

                                    for (int i = 0; i < cart.Count; i++)
                                    {
                                        GoodsOrder goodsOrder = new GoodsOrder();
                                        goodsOrder.UserID = userID;
                                        goodsOrder.GoodsID = cart[i].ID;
                                        goodsOrder.OrderID = orders[0].ID;

                                        await dataservice.PostGoodsOrdersAsync(goodsOrder);
                                    }

                                    NavigationService ns = NavigationService.GetNavigationService(this);
                                    ns.Navigate(new ResultPage(order, User));

                                } 
                                else
                                {
                                    FailsDisplay.Text = "Popisné číslo není z číslic!";
                                }
                            }
                        }
                        else
                        {
                            FailsDisplay.Text = "Telefon není z číslic!";
                        }
                    }
                    else
                    {
                        FailsDisplay.Text = "Nesprávný tvar emailu!";
                    }
                }
                else
                {
                    FailsDisplay.Text = "Není vše vyplněno!";
                }
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

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
