using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Models;
using DevClient.Services;

namespace DevClient.ViewModels
{
    public partial class OrderDishAndDrinksChangeVM : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<OrderDishAndDrinksChangeModel> _dishes;

        [ObservableProperty]
        private string[] _dishAndDrinksNames;

        [ObservableProperty]
        private string _selectedDishAndDrinkName;

        private OrderModel _order;

        public Action CloseAction { get; set; }

        public OrderDishAndDrinksChangeVM()
        {
            Dishes = new ObservableCollection<OrderDishAndDrinksChangeModel>();
            DishAndDrinksNames = Array.Empty<string>();
        }

        public void SetData(List<OrderDishAndDrinksChangeModel> data, string[] dishAndDrinksNames, OrderModel order)
        {
            Dishes.Clear();
            data.ForEach(x => Dishes.Add(x));
            _order = order;
            DishAndDrinksNames = dishAndDrinksNames; 
            SelectedDishAndDrinkName = DishAndDrinksNames.FirstOrDefault();
        }

        [RelayCommand]
        public async void ChangeOrder()
        {
            var request = await ApiService.UpdateOrderDishAndDrinks(new OrderDishAndDrinkUpdate
            {
                OrderId = _order.Id,
                AddDishAndDrinkToOrder = _dishes.Select(x => new NewOrderDishAndDrink
                {
                    IdOrderDishAndDrink = x.Id,
                    Count = x.Count,
                    Name = x.Name,
                }).ToArray(),
            });

            if (request.IsSuccesess)
            {
                _order.Price = request.Price;
                CloseAction?.Invoke();
            }
        }

        [RelayCommand]
        public void DeleteSelected()
        {
            var selected = Dishes.Where(x => x.IsSelected).ToList();
            selected.ForEach(x => Dishes.Remove(x));
        }

        [RelayCommand]
        public void AddDishToList()
        {
            var oldDish = Dishes.FirstOrDefault(x => x.Name == SelectedDishAndDrinkName);

            if (oldDish is not null)
            {
                oldDish.Count += 1;
                return;
            }

            Dishes.Add(new OrderDishAndDrinksChangeModel
            {
                ChangeAt = DateTime.UtcNow,
                Count = 1,
                Name = SelectedDishAndDrinkName,
            });
        }
    }
}
