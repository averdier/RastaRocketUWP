using RastaRocketUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input.Inking;
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
    public sealed partial class NeedAddPage : Page
    {
        public NeedAddPageViewModel ViewModel { get; } = new NeedAddPageViewModel();
        public NeedAddPage()
        {
            this.InitializeComponent();

            inkCanvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Pen;

            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();

            drawingAttributes.Color = Windows.UI.Colors.Black;
            drawingAttributes.IgnorePressure = false;
            drawingAttributes.FitToCurve = true;
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private async void InkRecognition()
        {
            IReadOnlyList<InkStroke> currentStrokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();

            if (currentStrokes.Count > 0)
            {
                InkRecognizerContainer inkRecognizerContainer = new InkRecognizerContainer();

                if (!(inkRecognizerContainer == null))
                {
                    IReadOnlyList<InkRecognitionResult> recognitionResults = await inkRecognizerContainer.RecognizeAsync(inkCanvas.InkPresenter.StrokeContainer, InkRecognitionTarget.All);

                    if (recognitionResults.Count > 0)
                    {
                        string completeResult = "";

                        foreach (var result in recognitionResults)
                        {
                            IReadOnlyList<string> candidates = result.GetTextCandidates();
                            if (candidates.Count > 0)
                            {
                                completeResult += candidates[0] + " ";
                            }
                        }

                        ViewModel.Description = completeResult;
                    }
                }
            }
        }

        private void DescriptionPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Changed");
            PivotItem item = ((PivotItem)((Pivot)sender).SelectedItem);
            if (item.Name == "ResultPivotItem")
            {
                InkRecognition();
            }
        }
    }
}
