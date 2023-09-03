using Business.Models;
using System.Threading.Tasks;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using DevClient.Services;

namespace DevClient.Models
{
    public partial class DiscountModel : ObservableObject
    {
        [ObservableProperty]
        private string _dishAndDrinkName;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private float _discountValue;

        [ObservableProperty]
        private float _discountPrice;

        [ObservableProperty]
        private DateTime _dateStart;

        [ObservableProperty]
        private DateTime _dateEnd;

        public int Id { get; set; }


        public async Task<bool> DeleteAsync()
        {
            return await ApiService.DeleteDiscount(Id);
        }

        public async Task<bool> UpdateAsync()
        {
            return await ApiService.UpdateDiscount(new DiscountViewModel
            {
                Id = Id,
                DateEnd = DateEnd,
                DateStart = DateStart,
                DiscountValue = DiscountValue,
                DishAndDrinkName = DishAndDrinkName,
                DiscountPrice = DiscountPrice,
            });
        }
    }
}
