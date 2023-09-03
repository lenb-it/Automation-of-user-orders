using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Printing;
using System.Threading.Tasks;
using Business.Constants;
using Business.Models;
using DevClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DevClient.Services
{
    public static class ApiService
    {
        static ApiService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:5000");
        }

        private static LogInResult _user;
        
        private static HttpClient _client;

        public static string Email => _user.Email;

        public static string UserName => _user.UserName;

        public static string Role => _user.Role ?? string.Empty;

        public static async Task<(bool, string)> LogIn(LogIn outPutModel)
        {
            var result = await _client.PostAsJsonAsync("/Account/LogIn", outPutModel);

            if (!result.IsSuccessStatusCode) 
                return (false, string.Empty);

            var json = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<LogInResult>(json);

            if (model is null || model.Role == RoleNames.User)
                return (false, model.Role);

            _user = model;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_user.Token}");

            return (true, _user.Role);
        }

        public static async Task<string[]> GetCategories()
        {
            var result = await _client.GetAsync("/Menu/GetDishAndDrinkCategories");

            var json = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<string[]>(json);

            return model ?? Array.Empty<string>();
        }

        #region Reservations

        public static async Task<bool> DeleteReservation(int id)
        {
            var result = await _client.DeleteAsync($"/Reservation/Delete?id={id}");

            return result.IsSuccessStatusCode;
        }

        public static async Task<bool> UpdateReservation(Reservation reservation)
        {
            var result = await _client.PutAsJsonAsync($"/Reservation/Update/", reservation);

            return result.IsSuccessStatusCode;
        }

        public static async Task<List<Reservation>> GetLastFiveHundredReservations()
        {
            var result = await _client.GetAsync("/Reservation/GetLastFiveHundred");

            var json = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<Reservation>>(json);

            return model ?? new List<Reservation>();
        }

        #endregion

        #region Orders

        [HttpGet]
        public static async Task<Statistics> GetStatistics(DateTime dateStart, DateTime dateEnd)
        {
            var result = await _client
                .GetAsync($"/Order/GetStatistics/{dateStart:s}/{dateEnd:s}");

            var json = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<Statistics>(json);

            return model ?? new Statistics();
        }

        public static async Task<ChequeInfo> GetChequeInfo(int id) 
        {
            var result = await _client.GetAsync($"/Order/GetChequeInfo?orderId={id}");

            var json = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ChequeInfo>(json);

            return model ?? new ChequeInfo();
        }

        public static async Task<List<Order>> GetLastFiveHundredOrders()
        {
            var result = await _client.GetAsync("/Order/GetLastFiveHundred");

            var json = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<Order>>(json);

            return model ?? new List<Order>();
        }

        public static async Task<(bool IsSuccesess, float Price)> UpdateOrderDishAndDrinks(OrderDishAndDrinkUpdate data)
        {
            var result = await _client.PutAsJsonAsync("/Order/UpdateOrderDishAndDrinks", data);
            var price = 0f;

            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                price = JsonConvert.DeserializeObject<float>(json);
            }

            return (result.IsSuccessStatusCode, price);
        }

        public static async Task<bool> DeleteOrder(int id)
        {
            var result = await _client.DeleteAsync($"/Order/Delete?id={id}");

            return result.IsSuccessStatusCode;
        }

        public static async Task<bool> UpdateOrder(Order order)
        {
            var result = await _client.PutAsJsonAsync($"/Order/Update/", order);

            return result.IsSuccessStatusCode;
        }

        public static async Task<List<OrderDishAndDrinkViewModel>> GetOrderDishAndDrinks(int id)
        {
            var result = await _client.GetAsync($"/Order/GetOrderDishAndDrinks?id={id}");

            var json = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<OrderDishAndDrinkViewModel>>(json);

            return model ?? new List<OrderDishAndDrinkViewModel>();
        }

        #endregion

        #region DishAndDrink

        public static async Task<bool> UpdateDishAndDrink(DishAndDrinkUpdate model)
        {
            var result = await _client.PutAsJsonAsync("/Menu/UpdateDishAndDrink", model);

            return result.IsSuccessStatusCode;
        }

        public static async Task<bool> AddDishAndDrink(DishAndDrinkAdd model)
        {
            var result = await _client.PostAsJsonAsync("/Menu/AddDishAndDrink", model);

            return result.IsSuccessStatusCode;
        }

        public static async Task<List<DishAndDrinkMenu>> GetMenu()
        {
            var result = await _client.GetAsync("/Menu/GetAllMenu");

            var json = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<DishAndDrinkMenu>>(json);

            return model ?? new List<DishAndDrinkMenu>();
        }

        public static async Task<string[]> GetDishAndDrinksName()
        {
            var result = await _client.GetAsync("/Menu/GetDishAndDrinksName");

            var json = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<string[]>(json);

            return model ?? Array.Empty<string>();
        }

        #endregion

        #region Discount

        public static async Task<bool> DiscountAddRange(DiscountAddRange model)
        {
            var result = await _client.PostAsJsonAsync("/Discount/AddRange", model);

            return result.IsSuccessStatusCode;
        }

        public static async Task<List<DiscountViewModel>> GetLastFiveHundredDiscounts()
        {
            var result = await _client.GetAsync("/Discount/GetLastFiveHundred");

            var json = await result.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<DiscountViewModel>>(json);

            return model ?? new List<DiscountViewModel>();
        }

        public static async Task<bool> UpdateDiscount(DiscountViewModel discountViewModel)
        {
            var result = await _client.PutAsJsonAsync($"/Discount/Update/", discountViewModel);

            return result.IsSuccessStatusCode;
        }

        public static async Task<bool> DeleteDiscount(int id)
        {
            var result = await _client.DeleteAsync($"/Discount/Delete?id={id}");

            return result.IsSuccessStatusCode;
        }

        #endregion
    }
}