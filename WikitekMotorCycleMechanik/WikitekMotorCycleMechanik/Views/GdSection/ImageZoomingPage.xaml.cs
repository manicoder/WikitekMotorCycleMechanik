using WikitekMotorCycleMechanik.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.View.GdSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageZoomingPage : ContentPage
    {
        public ImageZoomingPage(GdImageGD gdImage, string code)
        {
            try
            {
                gd_image = gdImage.gd_image;
                InitializeComponent();
                BindingContext = this;
                this.Title = code;
                
                this.Title = gdImage.image_name;
            }
            catch (System.Exception ex)
            {
            }
        }

        private string _gd_image;
        public string gd_image
        {
            get => _gd_image;
            set
            {
                _gd_image = value;
                OnPropertyChanged("gd_image");
            }
        }

        public void show_alert(string title, string message, bool btnCancel, bool btnOk)
        {
            //Working = true;
            //TitleText = title;
            //MessageText = message;
            //OkVisible = btnOk;
            //CancelVisible = btnCancel;
            //CancelCommand = new Command(() =>
            //{
            //    Working = false;
            //});
        }
    }
}