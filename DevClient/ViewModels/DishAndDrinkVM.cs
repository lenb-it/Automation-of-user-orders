using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Models;
using DevClient.Services;
using DevClient.Views;

namespace DevClient.ViewModels
{
    public partial class DishAndDrinkVM :ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<DishAndDrinkModel> _dishesAndDrinks;

        public DishAndDrinkVM()
        {
            DishesAndDrinks = new ObservableCollection<DishAndDrinkModel>();
        }

        public void SetData(List<DishAndDrinkModel> data)
        {
            DishesAndDrinks.Clear();
            data.ForEach(x => DishesAndDrinks.Add(x));
        }

        [RelayCommand]
        public async void AddAsync()
        {
            var categories = await ApiService.GetCategories();
            var viewModel = new AddDishAndDrinkVM();
            var view = new AddDishAndDrinkView();

            viewModel.CloseAction = () => view.Close();
            viewModel.SetData(categories);
            view.DataContext = viewModel;
            view.Show();
        }

        [RelayCommand]
        public async void UpdateSelectedAsync()
        {
            var selected = DishesAndDrinks.Where(x => x.IsSelected).ToList();

            foreach (var item in selected)
            {
                if (await item.UpdateAsync())
                    item.IsSelected = false;
            }

            if (selected.Any(x => x.IsSelected))
                MessageBox.Show("Выделенные элементы не удалось сохранить, попробуйте ещё раз!");
        }

        [RelayCommand]
        public async void ChangeCategorySelectedAsync()
        {
            var view = new ChangeCategoryView();
            var model = new ChangeCategoryVM();
            var categories = await ApiService.GetCategories(); 
            var selected = DishesAndDrinks
                .Where(x => x.IsSelected)
                .ToList();

            model.SetDishesAndDrinks(selected);
            model.Categories = categories;
            model.SelectedCategory = categories.FirstOrDefault();
            model.CloseAction = () => view.Close();

            view.DataContext = model;
            view.Show();
        }

        [RelayCommand]
        public async void DiscountSelectedAsync()
        {
            var view = new AddDiscountView();
            var model = new AddDiscountVM();
            var selected = DishesAndDrinks
                .Where(x => x.IsSelected)
                .ToList();

            model.SetDishesAndDrinks(selected);
            model.CloseAction = () => view.Close();
            view.DataContext = model;
            view.Show();
        }
    }
}
