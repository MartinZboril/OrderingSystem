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
    /// Interakční logika pro LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        dataService dataservice = new dataService();
        public LoginPage()
        {
            InitializeComponent();
        }

        public LoginPage(string message)
        {
            InitializeComponent();
            FailsDisplay.Text = message;
        }

        private async void Loginbutton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Email.Text) && !String.IsNullOrEmpty(Password.Password.ToString()))
            {
                ObservableCollection<User> users = new ObservableCollection<User>();
                users = await dataservice.GetUserData();
                var user = users.FirstOrDefault(p => p.Email == Email.Text);

                if (user != null)
                {
                    user.Password = Password.Password.ToString();

                    ObservableCollection<User> selectedUser = new ObservableCollection<User>();
                    selectedUser = await dataservice.LoginUserDataAsync(user);

                    if (selectedUser[0].ID != 0)
                    {
                        selectedUser[0].Password = Password.Password.ToString();
                        NavigationService navigation = NavigationService.GetNavigationService(this);
                        navigation.Navigate(new CatalogPage(selectedUser[0]));
                    }
                    else
                    {
                        FailsDisplay.Text = "Chybně zadané heslo!";
                        selectedUser.Clear();
                    }

                    selectedUser.Clear();
                }
                else
                {
                    FailsDisplay.Text = "Uživatel neexistuje!";
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

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new RegistrationPage());
        }
    }
}
