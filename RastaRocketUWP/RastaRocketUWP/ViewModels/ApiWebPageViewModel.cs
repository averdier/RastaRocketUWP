using RastaRocketUWP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastaRocketUWP.ViewModels
{
    public class ApiWebPageViewModel : Observable
    {
        private const string defaultUrl = "https://levasseurantoine.ovh/rastarocket/api/";

        private Uri _source;
        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        public ApiWebPageViewModel()
        {
            Source = new Uri(defaultUrl);
        }
    }
}
