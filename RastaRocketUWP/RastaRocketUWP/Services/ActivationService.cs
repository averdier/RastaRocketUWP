﻿using RastaRocketUWP.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace RastaRocketUWP.Services
{
    internal class ActivationService
    {
        private readonly App _app;
        private readonly UIElement _shell;
        private readonly Type _defaultNavItem;

        public ActivationService(App app, Type defaultNavItem, UIElement shell = null)
        {
            _app = app;
            _shell = shell ?? new Frame();
            _defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize things like registering background task before the app is loaded
                await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    Window.Current.Content = _shell;
                    NavigationService.Frame.NavigationFailed += (sender, e) =>
                    {
                        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
                    };
                    NavigationService.Frame.Navigated += OnFrameNavigated;
                    if (SystemNavigationManager.GetForCurrentView() != null)
                    {
                        SystemNavigationManager.GetForCurrentView().BackRequested += OnAppViewBackButtonRequested;
                    }
                }
            }

            var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }

                // Ensure the current window is active
                Window.Current.Activate();

                if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
                {
                    /*
                    System.Diagnostics.Debug.WriteLine("X:ActivationService update TitleBar");
                    var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                    if (titleBar != null)
                    {
                        titleBar.ButtonBackgroundColor = Helpers.Colors.GetSolidColorBrush("#FFFB8234").Color;
                        titleBar.ButtonForegroundColor = Colors.White;
                        titleBar.BackgroundColor = Helpers.Colors.GetSolidColorBrush("#FFFB8234").Color;
                        titleBar.ForegroundColor = Colors.White;
                    }*/
                }

                // Tasks after activation
                await StartupAsync();
            }
        }

        private async Task InitializeAsync()
        {
            //await ThemeSelectorService.InitializeAsync();
            await Task.CompletedTask;
        }

        private async Task StartupAsync()
        {
            //Services.ThemeSelectorService.SetRequestedTheme();
            await Task.CompletedTask;
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {

            yield break;
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = (NavigationService.CanGoBack) ?
                AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void OnAppViewBackButtonRequested(object sender, BackRequestedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
                e.Handled = true;
            }
        }
    }
}
