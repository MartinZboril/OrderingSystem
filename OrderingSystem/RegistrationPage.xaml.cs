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
    /// Interakční logika pro RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        dataService dataservice = new dataService();

        public RegistrationPage()
        {
            InitializeComponent();
        }

        private async void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {

            ObservableCollection<User> users = new ObservableCollection<User>();
            users = await dataservice.GetUserData();

            FailsDisplay.Text = "";
            if (!String.IsNullOrEmpty(Name.Text) && !String.IsNullOrEmpty(Surname.Text) && !String.IsNullOrEmpty(Email.Text) && !String.IsNullOrEmpty(Phone.Text) && !String.IsNullOrEmpty(Password.Password.ToString()) && !String.IsNullOrEmpty(ConfirmPassword.Password.ToString()))
            {
                if (Password.Password.ToString().Length > 7)
                {
                    if (Password.Password.ToString().Equals(ConfirmPassword.Password.ToString()))
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
                                    User user = new User();
                                    user.Name = Name.Text;
                                    user.Surname = Surname.Text;
                                    user.Password = Password.Password.ToString();
                                    user.Email = Email.Text;
                                    user.Phone = int.Parse(Phone.Text);

                                    ObservableCollection<User> selectedUser = new ObservableCollection<User>();
                                    selectedUser = await dataservice.PostUserDataAsync(user);

                                    NavigationService ns = NavigationService.GetNavigationService(this);
                                    ns.Navigate(new CatalogPage(selectedUser[0]));
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
                        FailsDisplay.Text = "Hesla se neshodují!";
                    }
                }
                else
                {
                    FailsDisplay.Text = "Heslo je příliš krátké!";
                }
            }
            else
            {
                FailsDisplay.Text = "Není vše vyplněno!";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new CatalogPage());
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new LoginPage());
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
