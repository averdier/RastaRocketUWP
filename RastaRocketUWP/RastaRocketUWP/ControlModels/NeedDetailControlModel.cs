using RastaRocketUWP.Extensions;
using RastaRocketUWP.Helpers;
using RastaRocketUWP.Models;
using RastaRocketUWP.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastaRocketUWP.ControlModels
{
    public class NeedDetailControlModel : Observable
    {
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

        private APIService _api;

        public NeedDetailControlModel()
        {
            IsLoading = false;
            _api = new APIService(Helpers.Settings.Username, Helpers.Settings.Password);
        }

        public async void OnMasterItemChanged(NeedModel item)
        {
            IsLoading = true;
            LoadingMessage = "Need_Loading".GetLocalized();

            try
            {
                Item = await _api.GetNeedFromIdWithRetryAsync(item.Id);
            }

            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task canceled");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            IsLoading = false;
            LoadingMessage = "";
        }
    }
}
