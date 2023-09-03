using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Models;

namespace DevClient.ViewModels
{
    public partial class ReservationVM : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ReservationModel> _userReservations;

        public ReservationVM()
        {
            _userReservations = new ObservableCollection<ReservationModel>();
        }

        public void SetData(List<ReservationModel> data)
        {
            UserReservations.Clear();
            data.ForEach(x => UserReservations.Add(x));
        }

        [RelayCommand]
        public async void UpdateSelectedAsync()
        {
            var selected = UserReservations.Where(x => x.IsSelected).ToList();

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
            var selected = UserReservations.Where(x => x.IsSelected).ToList();

            foreach (var item in selected)
            {
                if (await item.DeleteAsync())
                {
                    item.IsSelected = false;
                    UserReservations.Remove(item);
                }
            }

            if (selected.Any(x => x.IsSelected))
                MessageBox.Show("Выделенные элементы не удалось удалить, попробуйте ещё раз!");
        }
    }
}
