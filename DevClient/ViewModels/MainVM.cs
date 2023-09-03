using System;
using System.Linq;
using System.Windows.Controls;
using Business.Constants;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Models;
using DevClient.Services;
using DevClient.Views;

namespace DevClient.ViewModels
{
    public sealed partial class MainVM : ObservableObject
    {
        [ObservableProperty]
        private Page _currentPage;

        private Page _orderPage;
        private Page _reservationPage;
        private Page _dishAndDrinkPage;
        private Page _discountPage;
        private Page _statisticsPage;

        [ObservableProperty]
        private bool _discountIsEnabled;

        [ObservableProperty]
        private bool _statisticsIsEnabled;

        [ObservableProperty]
        private bool _dishesIsEnabled;

        public MainVM()
        {
            var isEnabled = ApiService.Role == RoleNames.Administrator;
            DiscountIsEnabled = isEnabled;
            DishesIsEnabled = isEnabled;
            StatisticsIsEnabled = isEnabled;
        }

        [RelayCommand]
        public async void Statistics()
        {
            CurrentPage = _statisticsPage ??= new StatisticsView();
            var model = new StatisticsVM();
            CurrentPage.DataContext = model;
        }

        [RelayCommand]
        public async void Discount()
        {
            CurrentPage = _discountPage ??= new DiscountView();
            var model = new DiscountVM();
            var discounts = await ApiService.GetLastFiveHundredDiscounts();

            model.SetData(discounts.Select(x => new DiscountModel
            {
                Id = x.Id,
                DateEnd = x.DateEnd,
                DateStart = x.DateStart,
                DiscountValue = x.DiscountValue,
                DishAndDrinkName = x.DishAndDrinkName,
                DiscountPrice = x.DiscountPrice,
            })
            .ToList());

            CurrentPage.DataContext = model;
        }

        [RelayCommand]
        public async void DishAndDrink()
        {
            CurrentPage = _dishAndDrinkPage ??= new DishAndDrinkView();
            var model = new DishAndDrinkVM();
            var dishesAndDrinks = await ApiService.GetMenu();

            model.SetData(dishesAndDrinks.Select(x => new DishAndDrinkModel
            {
                CategoryName = x.CategoryName,
                Description = x.Description,
                Id = x.Id,
                IsDiscount = x.Discount,
                IsValid = x.IsValid,
                Name = x.Name,
                Price = x.Price,
            })
            .ToList());

            CurrentPage.DataContext = model;
        }

        [RelayCommand]
        public async void Order()
        {
            CurrentPage = _orderPage ??= new OrderView();
            var model = new OrderVM();
            var orders = await ApiService.GetLastFiveHundredOrders();

            model.SetData(orders.Select(x => new OrderModel
            {
                Id = x.Id,
                Name = x.Name,
                IsPaid = x.IsPaid,
                Email = x.Email,
                Date = x.Date,
                NumberPlace = x.NumberPlace,
                Price = x.Price,
            })
            .ToList());

            CurrentPage.DataContext = model;
        }

        [RelayCommand]
        public async void Reservation()
        {
            CurrentPage = _reservationPage ??= new ReservationView();
            var model = new ReservationVM();
            var reservations = await ApiService.GetLastFiveHundredReservations();

            model.SetData(reservations.Select(x =>
                new ReservationModel
                {
                    CountPeople = x.CountPeople,
                    Date = new DateTime(x.Date.Year, x.Date.Month, x.Date.Day),
                    Time = new DateTime(1, 1, 1, x.Date.Hour, x.Date.Minute, 0),
                    Email = x.Email,
                    Id = x.Id,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    IsConfirmed = x.IsConfirmed,
                })
                .ToList());

            CurrentPage.DataContext = model;
        }
    }
}
