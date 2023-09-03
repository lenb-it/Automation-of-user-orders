using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using DevClient.Services;

namespace DevClient.Models
{
    public partial class DishAndDrinkModel : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private string _categoryName;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private bool _isDiscount;
        
        [ObservableProperty]
        private bool _isValid;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private float _price;

        public async Task<bool> UpdateAsync()
        {
            return await ApiService.UpdateDishAndDrink(new DishAndDrinkUpdate
            {
                Id = Id,
                CategoryName = _categoryName,
                Description = _description,
                IsValid = _isValid,
                Name = _name,
                Price = _price,
            });
        }
    }
}
