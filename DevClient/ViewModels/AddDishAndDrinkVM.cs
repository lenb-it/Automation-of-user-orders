using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Linq;
using Business.Models;
using DevClient.Services;

namespace DevClient.ViewModels
{
    public partial class AddDishAndDrinkVM : ObservableObject
    {
        [ObservableProperty]
        private string[] _categories;

        [ObservableProperty]
        private string _selectedCategory;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private float _price;

        public Action CloseAction { get; set; }

        public void SetData(string[] categories)
        {
            Categories = categories;
            SelectedCategory = Categories.FirstOrDefault();
        }

        [RelayCommand]
        public async void Add()
        {
            if (string.IsNullOrWhiteSpace(SelectedCategory))
                return;

            var model = new DishAndDrinkAdd
            {
                CategoryName = SelectedCategory,
                Description = Description,
                Name = Name,
                Price = Price,
                IsValid = true,
            };

            await ApiService.AddDishAndDrink(model);

            CloseAction?.Invoke();
        }
    }
}
