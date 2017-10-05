using RastaRocketUWP.Models;
using RastaRocketUWP.Services;
using RastaRocketUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace RastaRocketUWP.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class NeedDetailPage : Page
    {
        public NeedDetailViewModel ViewModel { get; } = new NeedDetailViewModel();
        public NeedDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.LoadData(e.Parameter as NeedModel);

            this.Loaded += NeedDetailPage_Loaded;
        }

        private void NeedDetailPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (WindowStates.CurrentState.Name == "WideState")
            {
                NavigationService.GoBack();
            }
        }
    }
}
