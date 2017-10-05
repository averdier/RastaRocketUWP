using RastaRocketUWP.Helpers;
using RastaRocketUWP.Models;
using System;
using System.Collections.Generic;
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

        public NeedDetailControlModel()
        {

        }
    }
}
