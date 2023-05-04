using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        static WebClient Client = new WebClient();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return null;

                var byteArray = Client.DownloadData(value.ToString());
                return ImageSource.FromStream(() => new MemoryStream(byteArray));
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }


    //public class ByteArrayToImageSourceConverter : IValueConverter
    //{ 
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) 
    //    { 
    //        ImageSource retSource = null; 
    //        if (value != null) 
    //        { 
    //            byte[] imageAsBytes = (byte[])value; 
    //            retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes)); 
    //        } 
    //        return retSource; 
    //    } 
    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) 
    //    { 
    //        throw new NotImplementedException(); 
    //    } 
    //}
}