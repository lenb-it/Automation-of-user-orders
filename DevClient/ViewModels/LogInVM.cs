using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;
using Business.Constants;
using Business.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevClient.Services;
using DevClient.Views;

namespace DevClient.ViewModels
{
    public sealed partial class LogInVM : ObservableValidator
    {
        [ObservableProperty]
        [Required]
        [NotifyCanExecuteChangedFor(nameof(LogInCommand))]
        private string? _email = "admin@mail.ru";

        [ObservableProperty]
        [Required]
        [MinLength(6)]
        [NotifyCanExecuteChangedFor(nameof(LogInCommand))]
        private string? _password;

        [ObservableProperty]
        private string _error;

        [RelayCommand(CanExecute = nameof(CanLogIn))]
        public async Task LogIn(Window thisWindow)
        {
            var model = new LogIn
            {
                Email = Email,
                Password = Password,
            };

            var result = await ApiService.LogIn(model);

            if (!result.Item1)
            {
                Error = result.Item2 == RoleNames.User
                    ? "У вас не достаточно прав!"
                    : "Проверьте введеные данные!";
                return;
            }

            var window = new MainView();
            window.Show();

            thisWindow?.Close();
        }

        public bool CanLogIn()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
