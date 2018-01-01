using Newtonsoft.Json;
using OrderingSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.DataService
{  
    public class dataService
    {
        HttpClient client = new HttpClient();

        public async Task<ObservableCollection<Goods>> GetGoodsData()
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Goods/GET", string.Empty));
            var response = await client.GetAsync(uri);
            ObservableCollection<Goods> goods = new ObservableCollection<Goods>();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                goods = JsonConvert.DeserializeObject<ObservableCollection<Goods>>(content);
            }

            return goods;
        }

        public async Task<ObservableCollection<User>> GetUserData()
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Users/GET", string.Empty));
            var response = await client.GetAsync(uri);
            ObservableCollection<User> users = new ObservableCollection<User>();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<ObservableCollection<User>>(content);
            }

            return users;
        }


        public async Task<ObservableCollection<User>> LoginUserDataAsync(User item)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Users/Login/", string.Empty));
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);
            ObservableCollection<User> users = new ObservableCollection<User>();

            if (response.IsSuccessStatusCode)
            {
                var content1 = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<ObservableCollection<User>>(content1);
            }

            return users;
        }


        public async Task ChangePasswordDataAsync(User item)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Users/ChangePassword/", string.Empty));
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);
            ObservableCollection<User> users = new ObservableCollection<User>();

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task UpdateOrder(Order item)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Orders/UPDATE", string.Empty));
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);
            ObservableCollection<Order> prders = new ObservableCollection<Order>();

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<ObservableCollection<Goods>> GetGoodsByIDData(string ID)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Goods/SELECT/" + ID, string.Empty));
            var response = await client.GetAsync(uri);
            ObservableCollection<Goods> goods = new ObservableCollection<Goods>();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                goods = JsonConvert.DeserializeObject<ObservableCollection<Goods>>(content);
            }

            return goods;
        }

        public async Task PostGoodsAsync(Goods item)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Goods/POST", string.Empty));

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<ObservableCollection<User>> PostUserDataAsync(User item)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Users/POST", string.Empty));

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            ObservableCollection<User> users = new ObservableCollection<User>();
            HttpResponseMessage response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                
                var content1 = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<ObservableCollection<User>>(content1);
            }

            return users;
        }

        public async Task UpdateUserDataAsync(User item)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Users/UPDATE", string.Empty));

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<ObservableCollection<Order>> GetOrdersData()
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Orders/GET", string.Empty));
            var response = await client.GetAsync(uri);
            ObservableCollection<Order> orders = new ObservableCollection<Order>();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<ObservableCollection<Order>>(content);
            }

            return orders;
        }

        public async Task<ObservableCollection<Order>> GetOrderByIDData(string OrderNumber)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Orders/SELECT/" + OrderNumber, string.Empty));
            var response = await client.GetAsync(uri);
            ObservableCollection<Order> order = new ObservableCollection<Order>();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                order = JsonConvert.DeserializeObject<ObservableCollection<Order>>(content);
            }

            return order;
        }

        public async Task DeleteOrderAsync(string ID)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Orders/DELETE/"+ID, string.Empty));

            var json = JsonConvert.SerializeObject(ID);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task RestoreOrderAsync(string ID)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Orders/RESTORE/" + ID, string.Empty));

            var json = JsonConvert.SerializeObject(ID);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task DeleteGoodsOrderAsync(string ID)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/GoodsOrder/DELETE/" + ID, string.Empty));

            var json = JsonConvert.SerializeObject(ID);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<ObservableCollection<Order>> PostOrdersAsync(Order item)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/Orders/POST", string.Empty));

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);
            ObservableCollection<Order> order = new ObservableCollection<Order>();
            if (response.IsSuccessStatusCode)
            {
                var content1 = await response.Content.ReadAsStringAsync();
                order = JsonConvert.DeserializeObject<ObservableCollection<Order>>(content1);
            }
            return order;
        }

        public async Task<ObservableCollection<GoodsOrder>> GetGoodsOrderData(int ID)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/GoodsOrder/SELECT/" + ID, string.Empty));
            var response = await client.GetAsync(uri);
            ObservableCollection<GoodsOrder> goodsOrders = new ObservableCollection<GoodsOrder>();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                goodsOrders = JsonConvert.DeserializeObject<ObservableCollection<GoodsOrder>>(content);
            }

            return goodsOrders;
        }

        public async Task PostGoodsOrdersAsync(GoodsOrder item)
        {
            var uri = new Uri(string.Format("http://martinzboril.cz/Eshop/API.php/GoodsOrder/POST", string.Empty));

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {

            }
        }
    }
}
