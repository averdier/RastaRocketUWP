using RastaRocketUWP.Helpers;
using RastaRocketUWP.Models;
using RastaRocketUWP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace RastaRocketUWP.ViewModels
{
    public class NeedDetailViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private APIService _api;

        public ICommand StateChangedCommand { get; private set; }
        public ICommand RefreshItemClickCommand { get; private set; }
        public ICommand EditItemClickCommand { get; private set; }
        public ICommand DeleteItemClickCommand { get; private set; }

        private NeedModel _item;
        public NeedModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
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

        public NeedDetailViewModel()
        {
            RefreshItemClickCommand = new RelayCommand<RoutedEventArgs>(OnRefreshItemClick);
            DeleteItemClickCommand = new RelayCommand<RoutedEventArgs>(OnDeleteItemClick);
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }

        public void LoadData(NeedModel item)
        {
            Item = item;
        }

        private async void OnRefreshItemClick(RoutedEventArgs args)
        {
            
        }

        private async void OnDeleteItemClick(RoutedEventArgs args)
        {
            
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                NavigationService.GoBack();
            }
        }
    }
}
