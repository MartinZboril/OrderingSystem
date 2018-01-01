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
    /// Interakční logika pro ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        dataService dataservice = new dataService();
        User User = new User();
        ObservableCollection<Goods> cart = new ObservableCollection<Goods>();
        public ProfilePage(User user, ObservableCollection<Goods> CartGoodsList)
        {
            InitializeComponent();
            User = user;
            Name.Text = User.Name;
            Surname.Text = User.Surname;
            Email.Text = User.Email;
            Phone.Text = User.Phone.ToString();
            Password.Password = User.Password;
            cart = CartGoodsList;
            GetValueForShopCartInfo(cart);
        }

        public ProfilePage(User user)
        {
            InitializeComponent();
            User = user;
            Name.Text = User.Name;
            Surname.Text = User.Surname;
            Email.Text = User.Email;
            Phone.Text = User.Phone.ToString();
            Password.Password = User.Password;
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
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new CatalogPage(User, cart));
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new OrderEditPage(User, cart));
        }

        private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Password.Password.ToString()) && !String.IsNullOrEmpty(NewPassword.Password.ToString()) && !String.IsNullOrEmpty(ConfirmPassword.Password.ToString()))
            {
                User.Password = Password.Password.ToString();

                ObservableCollection<User> selectedUser = new ObservableCollection<User>();
                selectedUser = await dataservice.LoginUserDataAsync(User);

                if (selectedUser[0].ID != 0)
                {
                    if (NewPassword.Password.ToString().Equals(ConfirmPassword.Password.ToString()))
                    {
                        selectedUser[0].Password = ConfirmPassword.Password.ToString();
                        dataservice.ChangePasswordDataAsync(selectedUser[0]);

                        string Message = "Heslo změněno, přihlaš te se novým heslem.";

                        NavigationService navigation = NavigationService.GetNavigationService(this);
                        navigation.Navigate(new LoginPage(Message));
                    }
                    else
                    {
                        FailsDisplay.Text = "Hesla se neshodují!";
                    }
                    selectedUser.Clear();
                }
                else
                {
                    FailsDisplay.Text = "Chybně zadané současné heslo!";
                    selectedUser.Clear();
                }
            }
            else
            {
                FailsDisplay.Text = "Není vše vyplněno!";
            }
        }

        private async void ChangeUserInformationButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<User> users = new ObservableCollection<User>();
            users = await dataservice.GetUserData();

            FailsDisplay.Text = "";
            if (!String.IsNullOrEmpty(Name.Text) && !String.IsNullOrEmpty(Surname.Text) && !String.IsNullOrEmpty(Email.Text) && !String.IsNullOrEmpty(Phone.Text))
            {
                if (Email.Text.Contains("@") && Email.Text.Contains("."))
                {
                    int phone = 0;
                    if (int.TryParse(Phone.Text, out phone))
                    {
                        bool contains = users.Any(p => p.Email == Email.Text);

                        if (User.Email.Equals(Email.Text))
                        {
                            User user = new User();
                            user.ID = User.ID;
                            user.Name = Name.Text;
                            user.Surname = Surname.Text;
                            user.Password = User.Password;
                            user.Email = Email.Text;
                            user.Phone = int.Parse(Phone.Text);

                            dataservice.UpdateUserDataAsync(user);

                            NavigationService ns = NavigationService.GetNavigationService(this);
                            ns.Navigate(new CatalogPage(user, cart));
                        }
                        else if (contains)
                        {
                            FailsDisplay.Text = "Email už je zabraný!";
                        }
                        else
                        {
                            User user = new User();
                            user.ID = User.ID;
                            user.Name = Name.Text;
                            user.Surname = Surname.Text;
                            user.Password = User.Password;
                            user.Email = Email.Text;
                            user.Phone = int.Parse(Phone.Text);

                            dataservice.UpdateUserDataAsync(user);

                            NavigationService ns = NavigationService.GetNavigationService(this);
                            ns.Navigate(new CatalogPage(user, cart));
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

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new CatalogPage(cart));
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new ProfilePage(User, cart));
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new CartPage(cart, User));
        }
    }
}
