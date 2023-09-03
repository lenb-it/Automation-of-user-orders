using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Models;

namespace DevClient.ViewModels
{
    public partial class ChangeCategoryVM : ObservableObject
    {
        [ObservableProperty]
        public string[] _categories;

        [ObservableProperty]
        public string _selectedCategory;

        public Action CloseAction { get; set; }

        public List<DishAndDrinkModel> _dishesAndDrinks;

        public void SetDishesAndDrinks(List<DishAndDrinkModel> dishesAndDrinks)
        {
            _dishesAndDrinks = dishesAndDrinks;
        }

        [RelayCommand]
        public void ChangeCategory()
        {
            if (string.IsNullOrWhiteSpace(SelectedCategory))
                return;

            _dishesAndDrinks.ForEach(x => x.CategoryName = _selectedCategory);
            CloseAction?.Invoke();
        }
    }
}
