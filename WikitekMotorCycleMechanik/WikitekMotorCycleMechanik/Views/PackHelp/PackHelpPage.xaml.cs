using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace WikitekMotorCycleMechanik.Views.PackHelp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PackHelpPage : ContentPage
    {
        public PackHelpPage()
        {
            InitializeComponent();
            //Device.BeginInvokeOnMainThread(() =>
            //{
                GetVideoContent();
            //});
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            MyMediaElement.Stop();
            base.OnDisappearing();
        }

        private async void GetVideoContent()
        {
            MyActivityIndicator.IsVisible = true;
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync("https://youtu.be/vH9O9ST9-2o");
            string title = video.Title; // "Downloaded Video Title"
            var author = video.Author; // "Downloaded Video Author"
            var duration = video.Duration;

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync("https://youtu.be/vH9O9ST9-2o");
            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

            if (streamInfo != null)
            {
                // Get the actual stream
                System.IO.Stream stream = await youtube.Videos.Streams.GetAsync(streamInfo);
                MyMediaElement.Source = streamInfo.Url;
            }
        }

        void MediaElement_MediaOpened(System.Object sender, EventArgs e)
        {
            MyActivityIndicator.IsVisible = false;
        }

        //async void CloseButton_Clicked(System.Object sender, EventArgs e)
        //{
        //    MyMediaElement.Stop();
        //    await Navigation.PopAsync();
        //}

        //private async void GetVideoContent()
        //{
        //    try
        //    {
        //        YoutubeClient youtube = new YoutubeClient();
        //        // You can specify video ID or URL
        //        YoutubeExplode.Videos.Video video = await youtube.Videos.GetAsync("https://www.youtube.com/watch?v=yJAKkz17WDo");
        //        string title = video.Title; // "Downloaded Video Title"
        //                                    //string author = video.Author; // "Downloaded Video Author"
        //        TimeSpan duration = (TimeSpan)video.Duration; // "Downloaded Video Duration Count"
        //                                                      //Now it's time to get stream :
        //        StreamManifest streamManifest = await youtube.Videos.Streams.GetManifestAsync("https://www.youtube.com/watch?v=yJAKkz17WDo");
        //        IVideoStreamInfo streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
        //        if (streamInfo != null)
        //        {
        //            // Get the actual stream
        //            System.IO.Stream stream = await youtube.Videos.Streams.GetAsync(streamInfo);
        //            mediasource.Source = streamInfo.Url;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}