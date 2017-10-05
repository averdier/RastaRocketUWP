using RastaRocketUWP.Helpers;
using RastaRocketUWP.Services;
using RastaRocketUWP.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace RastaRocketUWP.ViewModels
{
    public class LoginViewModel : Observable
    {
        private APIService _api;

        private string _email;
        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        private string _loadingMessage;
        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set { Set(ref _loadingMessage, value); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { Set(ref _errorMessage, value); }
        }

        public ICommand LoginClickCommand { get; private set; }

        public LoginViewModel()
        {
            LoginClickCommand = new RelayCommand<RoutedEventArgs>(OnLoginClick);
            IsLoading = false;
            ErrorMessage = "";
            _api = new APIService();
        }

        private async void OnLoginClick(RoutedEventArgs args)
        {
            Task<Boolean> loginTask = _api.LoginAsync(_email, _password);

            IsLoading = true;
            LoadingMessage = "Login, please wait ...";

            try
            {
                bool success = await loginTask;

                if (success)
                {
                    await Task.Run(() =>
                    {
                        Helpers.Settings.Username = _email;
                        Helpers.Settings.Password = _password;
                    });
                    NavigationService.Navigate(typeof(Views.ShellPage));
                }
                else
                {
                    IsLoading = false;
                    ErrorMessage = "Error, please try later";
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
                Debug.WriteLine("X:LoginViewModel exception" + ex.Message);
                ErrorMessage = ex.Message;

            }
        }
    }
}
