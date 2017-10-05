using RastaRocketUWP.ControlModels;
using RastaRocketUWP.Models;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace RastaRocketUWP.Controls
{
    public sealed partial class NeedDetailControl : UserControl
    {
        public NeedModel MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as NeedModel; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public int ThumbnailImageSideLength
        {
            get { return (int)GetValue(ThumbnailImageSideLengthProperty); }
            set { SetValue(ThumbnailImageSideLengthProperty, value); }
        }

        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(NeedModel), typeof(NeedDetailControl), new PropertyMetadata(null));
        public static DependencyProperty ThumbnailImageSideLengthProperty = DependencyProperty.Register("ThumbnailImageSideLength", typeof(int), typeof(NeedDetailControl), new PropertyMetadata(null));



        public NeedDetailControlModel ControlModel { get; private set; }


        public NeedDetailControl()
        {
            this.InitializeComponent();
        }
    }
}
