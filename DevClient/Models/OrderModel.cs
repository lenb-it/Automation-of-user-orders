using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Services;
using DevClient.ViewModels;
using DevClient.Views;

namespace DevClient.Models
{
    public partial class OrderModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private DateTime _date;

        [ObservableProperty]
        private float _price;

        [ObservableProperty]
        private bool _isPaid;

        [ObservableProperty]
        private int _numberPlace;

        public int Id { get; set; }

        [RelayCommand]
        public async void OpenChequeWindow()
        {
            var view = new ChequeView();
            var chequeInfo = await ApiService.GetChequeInfo(Id);
            var viewModel = new ChequeVM();

            IsPaid = true;
            viewModel.SetData(chequeInfo, ApiService.UserName, Id);
            viewModel.CloseAction = () => view.Close();
            view.DataContext = viewModel;
            view.Show();
        }

        [RelayCommand(CanExecute = nameof(CanOpenChangeWindow))]
        public async void OpenChangeWindow()
        {
            var dishes = await ApiService.GetOrderDishAndDrinks(Id);
            var dishesName = await ApiService.GetDishAndDrinksName();
            var view = new OrderDishAndDrinksChangeView();
            var model = new OrderDishAndDrinksChangeVM();

            model.SetData(dishes.Select(x => new OrderDishAndDrinksChangeModel
            {
                Id = x.Id,
                Name = x.Name,
                ChangeAt = x.ChangeAt,
                Count = x.Count,
            })
            .ToList(), dishesName, this);

            view.DataContext = model;
            model.CloseAction = () => view.Close();
            view.Show();
        }

        private bool CanOpenChangeWindow() => !_isPaid;

        [RelayCommand]
        public async Task<bool> DeleteAsync()
        {
            return await ApiService.DeleteOrder(Id);
        }

        [RelayCommand]
        public async Task<bool> UpdateAsync()
        {
            return await ApiService.UpdateOrder(new Order
            {
                IsPaid = _isPaid,
                Email = _email,
                Name = _name,
                Id = Id,
                NumberPlace = _numberPlace,
                Price = _price,
            });
        }
    }
}
