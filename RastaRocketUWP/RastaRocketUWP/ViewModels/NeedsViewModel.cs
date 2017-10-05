using RastaRocketUWP.Helpers;
using RastaRocketUWP.Models;
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
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
            AddItemClickCommand = new RelayCommand<RoutedEventArgs>(OnAddItemClick);
            DeleteItemClickCommand = new RelayCommand<RoutedEventArgs>(OnDeleteItemClick);
            _isLoading = false;
            _selected = new NeedModel();
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
            AddNeeds();
        }

        public void LoadDataAsync(VisualState currentState)
        {
            LoadingColumnSpan = (currentState.Name == NarrowStateName) ? 1 : 2;
            IsLoading = false;
            _currentState = currentState;
            OnPropertyChanged(nameof(IsViewState));
        }

        public void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {

        }

        private async void OnRefreshItemsClick(ItemClickEventArgs args)
        {
            
        }

        private void OnPullRefreshItems(EventArgs e)
        {
            AddNeeds();
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            
        }

        private void OnAddItemClick(RoutedEventArgs args)
        {
            
        }

        private async void OnDeleteItemClick(RoutedEventArgs args)
        {
            
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
                NeedsItems.Insert(0, new NeedModel { Title = "Item " + NeedsItems.Count, CreatedAt="2017-10-04 21:12" });
            }
        }

    }
}
