using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Models;
using DevClient.Services;

namespace DevClient.ViewModels
{
    public partial class AddDiscountVM : ObservableObject
    {
        [ObservableProperty]
        private DateTime _dateStart = DateTime.UtcNow;

        [ObservableProperty]
        private DateTime _dateEnd = DateTime.UtcNow;

        [ObservableProperty]
        private int _discount = 10;

        public Action CloseAction { get; set; }

        private List<DishAndDrinkModel> _dishesAndDrinks;

        public void SetDishesAndDrinks(List<DishAndDrinkModel> dishesAndDrinks)
        {
            _dishesAndDrinks = dishesAndDrinks;
        }

        [RelayCommand]
        public async void AddDiscount()
        {
            if (DateStart >= DateEnd || DateStart < DateTime.UtcNow)
            {
                MessageBox.Show("Даты введены не корректно");
                return;
            }

            var dateEnd = DateEnd.AddDays(1);
            var model = new DiscountAddRange
            {
                DateEnd = dateEnd,
                DateStart = DateStart,
                Discount = Discount,
                DishAndDrinkIds = _dishesAndDrinks.Select(x => x.Id).ToArray(),
            };

            var isAdded = await ApiService.DiscountAddRange(model);
            if (!isAdded)
                MessageBox.Show("Произошла ошибка при сохранении");
            else
                _dishesAndDrinks.ForEach(x => x.IsSelected = false);

            CloseAction?.Invoke();
        }
    }
}
