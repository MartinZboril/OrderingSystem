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
    /// Interakční logika pro OrderEditPage.xaml
    /// </summary>
    public partial class OrderEditPage : Page
    {
        dataService dataservice = new dataService();
        ObservableCollection<Goods> goods = new ObservableCollection<Goods>();
        ObservableCollection<GoodsOrder> goodsOrder = new ObservableCollection<GoodsOrder>();
        ObservableCollection<Goods> cart = new ObservableCollection<Goods>();
        Order Order = new Order();
        User User = new User();

        public OrderEditPage(User user, ObservableCollection<Goods> CartGoodsList)
        {
            InitializeComponent();
            User = user;
            cart = CartGoodsList;
            GetOrders(User);
            GetGoodsToBox();
            GetValueForShopCartInfo(cart);
        }

        public OrderEditPage(User user, ObservableCollection<Goods> CartGoodsList, string selectedtext)
        {
            InitializeComponent();
            User = user;
            cart = CartGoodsList;
            GetOrdersByBox(selectedtext, User);
            GetGoodsToBox();
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

        public async void GetGoodsToBox()
        {
            ObservableCollection<Goods> goods = new ObservableCollection<Goods>();
            goods = await dataservice.GetGoodsData();

            List<string> goodsName = new List<string>();

            for (int i = 0; i < goods.Count; i++)
            {
                goodsName.Add(goods[i].Name);
            }

            AddGoods.ItemsSource = goodsName;
        }

        public async Task GetOrders(User user)
        {
            ObservableCollection<Order> orders = new ObservableCollection<Order>();
            orders = await dataservice.GetOrdersData();

            ObservableCollection<Order> NotDeletedOrders = new ObservableCollection<Order>();

            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i].Deleted == 0 && orders[i].UserID == user.ID)
                {
                    if (orders[i].Price == 0)
                    {
                        await dataservice.DeleteOrderAsync(orders[i].ID.ToString());
                        NavigationService ns = NavigationService.GetNavigationService(this);
                        ns.Navigate(new OrderEditPage(User, cart));
                    }
                    else
                    {
                        NotDeletedOrders.Add(orders[i]);
                    }
                }
            }

            listView2.ItemsSource = NotDeletedOrders;
            listView2.Visibility = Visibility.Visible;
            listViewDeletedOrders.Visibility = Visibility.Hidden;
        }

        public async Task GetDeletedOrders(User user)
        {
            ObservableCollection<Order> orders = new ObservableCollection<Order>();
            orders = await dataservice.GetOrdersData();

            ObservableCollection<Order> DeletedOrders = new ObservableCollection<Order>();

            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i].Deleted == 1 && orders[i].UserID == user.ID)
                {
                    DeletedOrders.Add(orders[i]);
                }
            }

            listViewDeletedOrders.ItemsSource = DeletedOrders;
            listViewDeletedOrders.Visibility = Visibility.Visible;
            listView2.Visibility = Visibility.Hidden;
        }

        public async Task GetGoodsOrder(int ID)
        {
            if (ID != 0)
            {
                goods.Clear();
                goodsOrder.Clear();

                goodsOrder = await dataservice.GetGoodsOrderData(ID);

                ObservableCollection<GoodsOrder> NotDeletedGoodsOrders = new ObservableCollection<GoodsOrder>();

                for (int i = 0; i < goodsOrder.Count; i++)
                {
                    if (goodsOrder[i].Deleted == 0)
                    {
                        NotDeletedGoodsOrders.Add(goodsOrder[i]);
                    }
                }

                for (int i = 0; i < NotDeletedGoodsOrders.Count; i++)
                {
                    ObservableCollection<Goods> selectedGoods = new ObservableCollection<Goods>();
                    selectedGoods = await dataservice.GetGoodsByIDData(NotDeletedGoodsOrders[i].GoodsID.ToString());
                    selectedGoods[0].ID = NotDeletedGoodsOrders[i].ID;
                    goods.Add(selectedGoods[0]);
                }

                listView.ItemsSource = goods;
                listView.Visibility = Visibility.Visible;               
                listViewWithDeletedgoods.Visibility = Visibility.Hidden;
                AddGoods.IsHitTestVisible = true;
                AddGoods.Focusable = true;
            }
        }

        public async Task GetDeletedGoodsOrder(int ID)
        {
            if (ID != 0)
            {
                goods.Clear();
                goodsOrder.Clear();

                goodsOrder = await dataservice.GetGoodsOrderData(ID);

                ObservableCollection<GoodsOrder> NotDeletedGoodsOrders = new ObservableCollection<GoodsOrder>();

                for (int i = 0; i < goodsOrder.Count; i++)
                {
                    NotDeletedGoodsOrders.Add(goodsOrder[i]);
                }

                for (int i = 0; i < NotDeletedGoodsOrders.Count; i++)
                {
                    ObservableCollection<Goods> selectedGoods = new ObservableCollection<Goods>();
                    selectedGoods = await dataservice.GetGoodsByIDData(NotDeletedGoodsOrders[i].GoodsID.ToString());
                    selectedGoods[0].ID = NotDeletedGoodsOrders[i].ID;
                    goods.Add(selectedGoods[0]);
                }

                listViewWithDeletedgoods.ItemsSource = goods;
                listViewWithDeletedgoods.Visibility = Visibility.Visible;
                listView.Visibility = Visibility.Hidden;
            }
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new CatalogPage(User, cart));
        }

        private async void listView2_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            var order = listView2.SelectedItem as Order;
            if (order != null)
            {
                OrderNumber.Text = order.Number.ToString();
                OrderID.Text = order.ID.ToString();
                GetGoodsOrder(order.ID);
            }
        }

        private async void DeleteFormCart_Button_Click(object sender, RoutedEventArgs e)
        {
            string item = (e.Source as Button).Tag.ToString();
            await dataservice.DeleteGoodsOrderAsync(item);

            ObservableCollection<Order> getOrder = new ObservableCollection<Order>();
            getOrder = await dataservice.GetOrdersData();

            Order selectedOrder = new Order();

            for (int i = 0; i < getOrder.Count; i++)
            {
                if (getOrder[i].ID == int.Parse(OrderID.Text))
                {
                    selectedOrder = getOrder[i];
                }
            }

            ObservableCollection<Goods> choosenGoods = new ObservableCollection<Goods>();
            choosenGoods = await dataservice.GetGoodsData();

            ObservableCollection<GoodsOrder> goodsOrders = new ObservableCollection<GoodsOrder>();
            goodsOrders = await dataservice.GetGoodsOrderData(int.Parse(OrderID.Text));

            GoodsOrder selectedGoodsOrder = new GoodsOrder();

            for (int i = 0; i < goodsOrders.Count; i++)
            {
                if (goodsOrders[i].ID == int.Parse(item))
                {
                    selectedGoodsOrder = goodsOrders[i];
                }
            }

            Goods goodsDeleted = new Goods();

            for (int i = 0; i < choosenGoods.Count; i++)
            {
                if (selectedGoodsOrder.GoodsID == choosenGoods[i].ID)
                {
                    goodsDeleted = choosenGoods[i];
                }
            }

            Order updateOrder = new Order();
            updateOrder.ID = int.Parse(OrderID.Text);
            updateOrder.Price = selectedOrder.Price - goodsDeleted.Price;
            await dataservice.UpdateOrder(updateOrder);

            GetGoodsOrder(int.Parse(OrderID.Text));
            GetOrders(User);
          
            getOrder.Clear();
            choosenGoods.Clear();
            goodsOrders.Clear();
        }

        private async void DeleteOrder_Button_Click(object sender, RoutedEventArgs e)
        {
            string item = (e.Source as Button).Tag.ToString();
            await dataservice.DeleteOrderAsync(item);
            AddGoods.IsEditable = false;
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new OrderEditPage(User, cart));
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

        private async void OrderBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)OrderBox.SelectedItem;
            string selectedText = cbi.Content.ToString();

            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new OrderEditPage(User, cart, selectedText));
        }

        public async void GetOrdersByBox(string selectedText, User user)
        {
            if (selectedText.Equals("skryté"))
            {
                GetDeletedOrders(user);
                AddGoods.Visibility = Visibility.Hidden;
                OrderBoxText.Visibility = Visibility.Hidden;
                GetGoodsToBox();
            }
            else
            {
                GetOrders(user);
                AddGoods.Visibility = Visibility.Visible;
                OrderBoxText.Visibility = Visibility.Visible;
                GetGoodsToBox();
            }
        }

        private void listViewDeletedOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var order = listViewDeletedOrders.SelectedItem as Order;
            if (order != null)
            {
                OrderNumber.Text = order.Number.ToString();
                OrderID.Text = order.ID.ToString();
                GetDeletedGoodsOrder(order.ID);
            }
        }

        private async void RestoreOrder_Button_Click(object sender, RoutedEventArgs e)
        {
            string item = (e.Source as Button).Tag.ToString();
            await dataservice.RestoreOrderAsync(item);

            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(new OrderEditPage(User, cart));
        }

        private async void AddGoods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Goods> goods = new ObservableCollection<Goods>();
            goods = await dataservice.GetGoodsData();

            var selectedGoods = AddGoods.SelectedItem;

            Goods goodsToAdd = new Goods();

            for (int i = 0; i < goods.Count; i++)
            {
                if (goods[i].Name.Equals(selectedGoods))
                {
                    goodsToAdd = goods[i];
                }
            }

            if (goodsToAdd != null)
            {
                GoodsOrder goodsOrder = new GoodsOrder();
                goodsOrder.UserID = User.ID;
                goodsOrder.GoodsID = goodsToAdd.ID;
                goodsOrder.OrderID = int.Parse(OrderID.Text);

                await dataservice.PostGoodsOrdersAsync(goodsOrder);

                ObservableCollection<GoodsOrder> goodsOrders = new ObservableCollection<GoodsOrder>();
                goodsOrders = await dataservice.GetGoodsOrderData(int.Parse(OrderID.Text));

                ObservableCollection<GoodsOrder> NotDeletedGoodsOrders = new ObservableCollection<GoodsOrder>();
                ObservableCollection<Goods> goodsToUpdatePrice = new ObservableCollection<Goods>();

                for (int i = 0; i < goodsOrders.Count; i++)
                {
                    if (goodsOrders[i].Deleted == 0)
                    {
                        NotDeletedGoodsOrders.Add(goodsOrders[i]);
                    }
                }

                int price = 0;

                for (int i = 0; i < NotDeletedGoodsOrders.Count; i++)
                {
                    ObservableCollection<Goods> choosenGoods = new ObservableCollection<Goods>();
                    choosenGoods = await dataservice.GetGoodsByIDData(NotDeletedGoodsOrders[i].GoodsID.ToString());
                    choosenGoods[0].ID = NotDeletedGoodsOrders[i].ID;
                    goodsToUpdatePrice.Add(choosenGoods[0]);
                    price += goodsToUpdatePrice[i].Price;
                }

                ObservableCollection<Order> getOrder = new ObservableCollection<Order>();
                getOrder = await dataservice.GetOrderByIDData(OrderID.Text);

                Order updateOrder = new Order();
                updateOrder.ID = int.Parse(OrderID.Text);
                updateOrder.Price = price;
                await dataservice.UpdateOrder(updateOrder);
            }

            GetGoodsOrder(int.Parse(OrderID.Text));
            GetOrders(User);
        }
    }
}