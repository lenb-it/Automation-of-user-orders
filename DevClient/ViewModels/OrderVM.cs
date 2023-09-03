using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Models;

namespace DevClient.ViewModels
{
    public partial class OrderVM : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<OrderModel> _orders;

        public OrderVM()
        {
            _orders = new ObservableCollection<OrderModel>();
        }

        public void SetData(List<OrderModel> data)
        {
            Orders.Clear();
            data.ForEach(x => Orders.Add(x));
        }

        [RelayCommand]
        public async void UpdateSelectedAsync()
        {
            var selected = Orders.Where(x => x.IsSelected).ToList();

            foreach (var item in selected)
            {
                if (await item.UpdateAsync())
                    item.IsSelected = false;
            }

            if (selected.Any(x => x.IsSelected))
                MessageBox.Show("Выделенные элементы не удалось сохранить, попробуйте ещё раз!");
        }

        [RelayCommand]
        public async void DeleteSelectedAsync()
        {
            var selected = Orders.Where(x => x.IsSelected).ToList();

            foreach (var item in selected)
            {
                if (await item.DeleteAsync())
                {
                    item.IsSelected = false;
                    Orders.Remove(item);
                }
            }

            if (selected.Any(x => x.IsSelected))
                MessageBox.Show("Выделенные элементы не удалось удалить, попробуйте ещё раз!");
        }
    }
}
