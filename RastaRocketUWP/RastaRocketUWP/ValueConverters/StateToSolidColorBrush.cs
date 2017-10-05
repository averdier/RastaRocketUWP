using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace RastaRocketUWP.ValueConverters
{
    public class StateToSolidColorBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                string formatString = value as string;
                if (!string.IsNullOrEmpty(formatString))
                {
                    if (formatString.Equals("open"))
                    {
                        return Helpers.Colors.GetSolidColorBrush("#FF337AB7");
                    }
                    else if (formatString.Equals("win"))
                    {
                        return Helpers.Colors.GetSolidColorBrush("#FF5CB85C");
                    }
                    else if (formatString.Equals("lost"))
                    {
                        return Helpers.Colors.GetSolidColorBrush("#FFD9534F");
                    }
                }
            }

            return new SolidColorBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
