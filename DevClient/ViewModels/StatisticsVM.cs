using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Services;

namespace DevClient.ViewModels
{
    public partial class StatisticsVM : ObservableObject
    {
        public StatisticsVM()
        {
            var now = DateTime.Now;
            DateStart = new DateTime(now.Year, now.Month, 1);
            DateEnd = now;
            Dishes = new ObservableCollection<StatisticDish>();
        }

        [ObservableProperty] 
        private DateTime _dateStart;

        [ObservableProperty]
        private DateTime _dateEnd;

        [ObservableProperty]
        private ObservableCollection<StatisticDish> _dishes;

        [ObservableProperty]
        private float _averageOrderPrice;

        [ObservableProperty]
        private int _countOrders;

        [ObservableProperty]
        private int _countReservations;

        [ObservableProperty]
        private float _minOrderPrice;

        [ObservableProperty]
        private float _maxOrderPrice;

        [RelayCommand]
        public async void LoadAsync()
        {
            var statistics = await ApiService.GetStatistics(DateStart, DateEnd);

            Dishes.Clear();
            statistics.Dishes.OrderByDescending(x => x.Count)
                .ToList()
                .ForEach(x => Dishes.Add(x));
            
            AverageOrderPrice = statistics.AverageOrderPrice;
            CountOrders = statistics.CountOrders;
            CountReservations = statistics.CountReservations;
            MinOrderPrice = statistics.MinOrderPrice;
            MaxOrderPrice = statistics.MaxOrderPrice;
        }
    }
}
