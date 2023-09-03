using System;
using System.Threading.Tasks;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using DevClient.Services;

namespace DevClient.Models
{
    public partial class ReservationModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _phoneNumber;

        [ObservableProperty]
        private DateTime _date;

        [ObservableProperty]
        private DateTime _time;

        [ObservableProperty]
        private bool _isConfirmed;

        public int Id { get; set; }

        [ObservableProperty]
        private int _countPeople;

        public async Task<bool> DeleteAsync()
        {
            return await ApiService.DeleteReservation(Id);
        }

        public async Task<bool> UpdateAsync()
        {
            var res = await ApiService.UpdateReservation(new Reservation
            {
                CountPeople = _countPeople,
                Date = new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, 0),
                Email = _email,
                Name = _name,
                Id = Id,
                IsConfirmed = _isConfirmed,
                PhoneNumber = _phoneNumber,
            });
            return res;
        }
    }
}
