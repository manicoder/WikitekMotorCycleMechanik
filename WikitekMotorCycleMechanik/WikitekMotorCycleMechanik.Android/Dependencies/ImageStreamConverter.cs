//using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Droid.Dependencies;
using WikitekMotorCycleMechanik.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageStreamConverter))]
namespace WikitekMotorCycleMechanik.Droid.Dependencies
{
    public class ImageStreamConverter : IImageStreamConverter
    {
        public async Task<byte[]> ImageStream()
        {
            //string imagePath = "WikitekMotorCycleMechanik\WikitekMotorCycleMechanik.Android\Resources\drawable.png";
            try
            {
                string imagefileName = "ic_demo_image.png";
                // Remove the file extention from the image filename
                imagefileName = imagefileName.Replace(".jpg", "").Replace(".png", "");

                // Retrieving the local Resource ID from the name
                int id = (int)typeof(Resource.Drawable).GetField(imagefileName).GetValue(null);

                // Converting Drawable Resource to Bitmap
                var originalImage = BitmapFactory.DecodeResource(Xamarin.Forms.Forms.Context.Resources, id);

                //var context = Xamarin.Forms.Forms.Context; //Application.Context;
               // using (var drawable = Xamarin.Forms.Platform.Android.ResourceManager.GetDrawable(context, "ic_demo.png"))
                using (var bitmap = ((Bitmap)originalImage))
                {
                    var stream = new MemoryStream();
                    bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                    bitmap.Recycle();
                    return stream.ToArray();
                }
                //Assembly assembly = typeof(ImageStreamConverter).GetTypeInfo().Assembly;

                //byte[] buffer;
                //byte[] myBinary;
                //using (Stream stream = assembly.GetManifestResourceStream("ic_demo.png"))
                //{
                //    long length = stream.Length;
                //    buffer = new byte[length];
                //    stream.Read(buffer, 0, (int)length);
                //    myBinary = new byte[stream.Length];
                //    stream.Read(myBinary, 0, (int)stream.Length);
                //    //var storeragePath = await iStorageService.SaveBinaryObjectToStorageAsync(string.Format(FileNames.ApplicationIcon, app.ApplicationId), buffer);
                //    //app.IconURLLocal = storeragePath;
                //}
                //return myBinary;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
    }
}