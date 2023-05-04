using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class AddColleborateViewModel : ViewModelBase
    {
        ApiServices apiServices;
        MediaFile img_file = null;
        MediaFile vid_file = null;
        CreateCollaborateModel model;
        string dtc_code;
        public AddColleborateViewModel(Page page, string dtc_code) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                model = new CreateCollaborateModel();
                this.dtc_code = dtc_code;
            }
            catch (Exception ex)
            {
            }
        }


        #region Properties
        private double _description_height = 250;
        public double description_height
        {
            get => _description_height;
            set
            {
                _description_height = value;
                OnPropertyChanged("description_height");
            }
        }

        private double _img_row_height = 0;
        public double img_row_height
        {
            get => _img_row_height;
            set
            {
                _img_row_height = value;
                OnPropertyChanged("img_row_height");
            }
        }

        private string _description;
        public string description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        private ImageSource _user_profile_pic;
        public ImageSource user_profile_pic
        {
            get => _user_profile_pic;
            set
            {
                _user_profile_pic = value;
                OnPropertyChanged("user_profile_pic");
            }
        }

        private string _vid_path;
        public string vid_path
        {
            get => _vid_path;
            set
            {
                _vid_path = value;
                OnPropertyChanged("vid_path");
            }
        }


        private string _plus_img;
        public string plus_img
        {
            get => _plus_img;
            set
            {
                _plus_img = value;
                OnPropertyChanged("plus_img");

                if (App.user_type == "wikitekMechanik")
                {
                    plus_img = "ic_add_wiki.png";
                }
                else if (App.user_type == "rsangleMechanik")
                {
                    plus_img = "ic_add_rsa.png";
                }
                else if (App.user_type == "mobitekMechanik")
                {
                    plus_img = "ic_add_mobi.png";
                }
            }
        }

        private bool _hide_delete_img = false;
        public bool hide_delete_img
        {
            get => _hide_delete_img;
            set
            {
                _hide_delete_img = value;
                OnPropertyChanged("hide_delete_img");
            }
        }

        private bool _hide_delete_vid = false;
        public bool hide_delete_vid
        {
            get => _hide_delete_vid;
            set
            {
                _hide_delete_vid = value;
                OnPropertyChanged("hide_delete_vid");
            }
        }
        #endregion

        #region ICommands
        public ICommand AddImageCommand => new Command(async (obj) =>
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await page.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                img_file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "profile.jpg",
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                    MaxWidthHeight = 70,
                    CompressionQuality = 50,
                });

                if (img_file == null)
                    return;

                //await page.DisplayAlert("File Location", file.Path, "OK");

                user_profile_pic = ImageSource.FromFile(img_file.Path);
                img_row_height = 200;
                hide_delete_img = true;
                //user_profile_pic = ImageSource.FromStream(() =>
                //{
                //    var stream = file.GetStream();
                //    return stream;
                //});
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand AddVideoCommand => new Command(async (obj) =>
        {
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            try
            {
                //using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    //await Task.Delay(100);
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickVideoSupported)
                   {
                       await page.DisplayAlert("No Camera", ":( No camera available.", "OK");
                       return;
                   }

                    vid_file = await CrossMedia.Current.PickVideoAsync();

                    if (vid_file == null)
                        return;

                    FileInfo fi = new FileInfo(vid_file.Path);

                    if (fi.Length > 10000000)
                    {
                        await page.DisplayAlert("Alert", "You can't upload a video up to size 10 MB.", "OK");
                        return;
                    }

                    vid_path = vid_file.Path;
                    //vid_file.Dispose();
                    img_row_height = 200;
                    hide_delete_vid = true;
                }
            }
            catch (Exception ex)
            {
            }
            //});

        });

        public ICommand DeleteImgCommand => new Command(async (obj) =>
        {
            try
            {
                hide_delete_img = false;
                user_profile_pic = null;
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand PostCollCommand => new Command(async (obj) =>
        {
            try
            {
                model = new CreateCollaborateModel
                {
                    timestamp = DateTime.Now.ToString("dd/MM/yyyy hh:mm"),
                    hashtags = string.Empty,
                    parent= string.Empty,
                    Pid_tag= dtc_code,
                    Tweet_tag= string.Empty,
                    reply_to= string.Empty,
                    user = App.user_id,
                };
                await apiServices.CreateCollaborate(img_file, vid_file, model);
            }
            catch (Exception ex)
            {
            }
        });
        #endregion
    }
}
