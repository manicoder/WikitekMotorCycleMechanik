using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Converters
{
    public class DropDownTextColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString().Contains("Select"))
                {
                    return (Color)Application.Current.Resources["placeholder_color"];
                }
                else
                {
                    return (Color)Application.Current.Resources["text_color"];
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
