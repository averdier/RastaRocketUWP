using RastaRocketUWP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RastaRocketUWP.ViewModels
{
    public class UploadViewModel : Observable
    {
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

        private string _selectedFileText;
        public string SelectedFileText
        {
            get { return _selectedFileText; }
            set { Set(ref _selectedFileText, value); }
        }

        public ICommand UploadClickCommand;

        public ICommand NeedSelectionChangedCommand { get; private set; }

        public ICommand FolderClickCommand { get; private set; }
    }
}
