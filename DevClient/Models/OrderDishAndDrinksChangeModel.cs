using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DevClient.Models
{
    public partial class OrderDishAndDrinksChangeModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isSelected;

        public int Id { get; set; }

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private DateTime _changeAt;

        [ObservableProperty]
        private float _count;
    }
}
