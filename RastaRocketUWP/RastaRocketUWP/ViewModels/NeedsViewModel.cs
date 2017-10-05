using RastaRocketUWP.Extensions;
using RastaRocketUWP.Helpers;
using RastaRocketUWP.Models;
using RastaRocketUWP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RastaRocketUWP.ViewModels
{
    public class NeedsViewModel : Observable
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private APIService _api;

        private VisualState _currentState;

        public ICommand StateChangedCommand { get; private set; }

        public bool IsViewState { get { return Selected != null && !IsLoading && _currentState.Name != NarrowStateName; } }

        public ObservableCollection<NeedModel> NeedsItems { get; private set; } = new ObservableCollection<NeedModel>();

        private NeedModel _selected;

        public NeedModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
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

        private int _loadingColumnSpan;
        public int LoadingColumnSpan
        {
            get { return _loadingColumnSpan; }
            set { Set(ref _loadingColumnSpan, value); }
        }

        public ICommand RefreshItemsClickCommand { get; private set; }
        public ICommand PullRefreshItemsCommand { get; private set; }

        public ICommand ItemClickCommand { get; private set; }

        public ICommand AddItemClickCommand { get; private set; }

        public ICommand DeleteItemClickCommand { get; private set; }

        public NeedsViewModel()
        {
            RefreshItemsClickCommand = new RelayCommand<ItemClickEventArgs>(OnRefreshItemsClick);
            PullRefreshItemsCommand = new RelayCommand<EventArgs>(OnPullRefreshItems);
            AddItemClickCommand = new RelayCommand<RoutedEventArgs>(OnAddItemClick);
            DeleteItemClickCommand = new RelayCommand<RoutedEventArgs>(OnDeleteItemClick);
            _isLoading = false;
            _selected = new NeedModel();
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }

        public async Task LoadDataAsync(VisualState currentState)
        {
            LoadingColumnSpan = (currentState.Name == NarrowStateName) ? 1 : 2;
            IsLoading = true;
            OnPropertyChanged(nameof(IsViewState));
            _currentState = currentState;
            NeedsItems.Clear();

            try
            {
                LoadingMessage = "Needs_Loading".GetLocalized();
                var data = await _api.GetNeedContainerWithRetryAsync();

                foreach (var item in data.Needs)
                {
                    NeedsItems.Add(item);
                }

                if (NeedsItems.Count > 0)
                {
                    Selected = NeedsItems[0];
                }

                IsLoading = false;
                OnPropertyChanged(nameof(IsViewState));
            }
            catch (Exception ex)
            {
                IsLoading = false;
                var errorDialog = new Windows.UI.Popups.MessageDialog(
                            ex.Message,
                            "Erreur");
                errorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });
                await errorDialog.ShowAsync();
            }
        }

        public void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            
        }

        private async void OnRefreshItemsClick(ItemClickEventArgs args)
        {
            await LoadDataAsync(_currentState);
        }

        private void OnPullRefreshItems(EventArgs e)
        {
            
        }

        public void OnItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("OnItemClick");
            NeedModel item = e?.ClickedItem as NeedModel;
            if (item != null)
            {
                if (_currentState.Name == NarrowStateName)
                {
                    NavigationService.Navigate<Views.NeedDetailPage>(item);
                }
                else
                {
                    Selected = item;
                    OnPropertyChanged(nameof(IsViewState));
                }
            }
        }

        private void OnAddItemClick(RoutedEventArgs args)
        {
            NavigationService.Navigate<Views.NeedAddPage>();
        }

        private async void OnDeleteItemClick(RoutedEventArgs args)
        {
            if (Selected != null)
            {
                var dialog = new Windows.UI.Popups.MessageDialog(
                    "NeedDeleteConfirm_Text".GetLocalized(),
                    "NeedDeleteConfirm_Title".GetLocalized()
                    );
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("NeedDeleteConfirm_Primary".GetLocalized()) { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("NeedDeleteConfirm_Secondary".GetLocalized()) { Id = 1 });

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                var result = await dialog.ShowAsync();

                if ((int)result.Id == 0)
                {
                    try
                    {
                        if (await _api.DeleteNeedWithRetryAsync(Selected.Id))
                        {
                            NeedsItems.Remove(Selected);
                            Selected = NeedsItems.FirstOrDefault();
                            OnPropertyChanged(nameof(IsViewState));
                        }
                        else
                        {
                            var unknowErrorDialog = new Windows.UI.Popups.MessageDialog(
                                "NeedDeleteConfirmError_Text".GetLocalized(),
                                "ModalError_Title".GetLocalized());
                            unknowErrorDialog.Commands.Add(new Windows.UI.Popups.UICommand("ModalError_Confirm".GetLocalized()) { Id = 0 });
                            await unknowErrorDialog.ShowAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorDialog = new Windows.UI.Popups.MessageDialog(
                            ex.Message,
                            "ModalError_Title".GetLocalized());
                        errorDialog.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
                        await errorDialog.ShowAsync();
                    }
                }
            }
        }


        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            _currentState = args.NewState;
            OnPropertyChanged(nameof(IsViewState));
            LoadingColumnSpan = (_currentState.Name == NarrowStateName) ? 1 : 2;
            Debug.WriteLine("StateChanged");
        }

        private void AddNeeds()
        {
            for (int i = 0; i < 10; i++)
            {
                NeedsItems.Insert(0, new NeedModel { Title = "Item " + NeedsItems.Count, Created_At="2017-10-04 21:12" });
            }
        }

    }
}
