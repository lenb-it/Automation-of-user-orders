using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Models;

namespace DevClient.ViewModels
{
    public partial class DiscountVM : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<DiscountModel> _discounts;

        public DiscountVM()
        {
            _discounts = new ObservableCollection<DiscountModel>();
        }

        public void SetData(List<DiscountModel> data)
        {
            Discounts.Clear();
            data.ForEach(x => Discounts.Add(x));
        }

        [RelayCommand]
        public async void UpdateSelectedAsync()
        {
            var selected = Discounts.Where(x => x.IsSelected).ToList();

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
            var selected = Discounts.Where(x => x.IsSelected).ToList();

            foreach (var item in selected)
            {
                if (await item.DeleteAsync())
                {
                    item.IsSelected = false;
                    Discounts.Remove(item);
                }
            }

            if (selected.Any(x => x.IsSelected))
                MessageBox.Show("Выделенные элементы не удалось удалить, попробуйте ещё раз!");
        }
    }
}
