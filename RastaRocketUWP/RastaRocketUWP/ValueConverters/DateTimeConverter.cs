using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace RastaRocketUWP.ValueConverters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                var strDateTime = (string)value;
                if (strDateTime != null)
                {
                    DateTime dateTime;
                    if (DateTime.TryParse(strDateTime, out dateTime))
                    {
                        return dateTime.ToString("dd/MM/yyyy HH:mm");
                    }

                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
