using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DevClient.ViewModels
{
    public partial class ChequeVM : ObservableObject
    {
        [ObservableProperty]
        private string _seller;

        [ObservableProperty]
        private string _dateString;

        [ObservableProperty]
        private int _orderId;

        [ObservableProperty]
        private float _totalPrice;

        [ObservableProperty]
        private List<ChequeDish> _dishes;

        public Action CloseAction { get; set; }

        public void SetData(ChequeInfo chequeInfo, string seller, int orderId)
        {
            Dishes = chequeInfo.ChequeDishes;
            Seller = seller;
            DateString = chequeInfo.Date.ToString("d");
            OrderId = orderId;
            TotalPrice = chequeInfo.TotalPrice;
        }

        [RelayCommand]
        public void PrintCheque(Visual visual)
        {
            try
            {
                var dialog = new PrintDialog();

                if (dialog.ShowDialog() == true)
                    dialog.PrintVisual(visual, "Чек");

                CloseAction?.Invoke();
            }
            catch(Exception)
            {
                MessageBox.Show("Произошла ошибка при печати");
            }
        }
    }
}
