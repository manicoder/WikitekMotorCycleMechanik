using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Converters
{
    public class ByteToImageFieldConverter : IValueConverter
    {
        static WebClient Client = new WebClient();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value == null)
            //    return null;

            
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {

                    var byteArray = Client.DownloadData(value.ToString());
                    return ImageSource.FromStream(() => new MemoryStream(byteArray));
                }
                else
                {
                    //var assembly = this.GetType().GetTypeInfo().Assembly; // you can replace "this.GetType()" with "typeof(MyType)", where MyType is any type in your assembly.
                    //byte[] buffer;
                    //using (Stream s = assembly.GetManifestResourceStream("ic_demo.png"))
                    //{
                    //    long length = s.Length;
                    //    buffer = new byte[length];
                    //    s.Read(buffer, 0, (int)length);
                    //    return s;
                    //}

                   var res =  Task.Run(async () =>
                    { return await DependencyService.Get<Interfaces.IImageStreamConverter>().ImageStream(); });

                    //new MemoryStream(res);

                   return ImageSource.FromStream(() => new MemoryStream(res.Result));
                }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //static WebClient client = new WebClient();
        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    string retSource;
        //    //Uri retSource = null;






        //    //return ImageSource.FromStream(() => new MemoryStream(client.DownloadData(value.ToString())));
        //    //    retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
        //    //}

        //    var current = Connectivity.NetworkAccess;
        //    if (current == NetworkAccess.Internet)
        //    {
        //        retSource = (string)value;
        //    }
        //    else
        //    {
        //        retSource = "icon.png";
        //    }
        //    return retSource;
        //}
        //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
    }
}