using Android.Content;
using WikitekMotorCycleMechanik.Droid.Dependencies;
using WikitekMotorCycleMechanik.Interfaces;
using Supremes;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;

[assembly: Dependency(typeof(LatestVersionCheck))]
namespace WikitekMotorCycleMechanik.Droid.Dependencies
{
    public class LatestVersionCheck : ILatest
    {
        string _packageName => global::Android.App.Application.Context.PackageName;
        string _versionName => global::Android.App.Application.Context.PackageManager.GetPackageInfo(global::Android.App.Application.Context.PackageName, 0).VersionName;

        /// <inheritdoc />
        public string InstalledVersionNumber
        {
            get => _versionName;
        }

        /// <inheritdoc />
        public async Task<bool> IsUsingLatestVersion()
        {
            bool isLatest = false;
            var latestVersion = string.Empty;

            try
            {
                latestVersion = await GetLatestVersionNumber();
                isLatest = Version.Parse(latestVersion).CompareTo(Version.Parse(_versionName)) <= 0;
            }
            catch (Exception e)
            {
                // throw new LatestVersionException($"Error comparing current app version number with latest. Version name={_versionName} and lastest version={latestVersion} .", e);
            }
            return isLatest;
        }

        /// <inheritdoc />
        public async Task<string> GetLatestVersionNumber()
        {
            return await GetLatestVersionNumber(_packageName);
        }

        /// <inheritdoc />
        /// 

        //public static string GetAndroidStoreAppVersion()
        //{
        //    string androidStoreAppVersion = null;

        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            var doc = client.GetAsync("https://play.google.com/store/apps/details?id=" + ""+ "&hl=en_CA").Result.Parse();
        //            var versionElement = doc.Select("div:containsOwn(Current Version)");

        //            androidStoreAppVersion = versionElement.Text;

        //            Element headElement = versionElement[0];
        //            Elements siblingsOfHead = headElement.SiblingElements;
        //            Element contentElement = siblingsOfHead.First;
        //            Elements childrenOfContentElement = contentElement.Children;
        //            Element childOfContentElement = childrenOfContentElement.First;
        //            Elements childrenOfChildren = childOfContentElement.Children;
        //            Element childOfChild = childrenOfChildren.First;

        //            androidStoreAppVersion = childOfChild.Text;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // do something
        //        Console.WriteLine(ex.Message);
        //    }

        //    return androidStoreAppVersion;
        //}

        public async Task<string> GetLatestVersionNumber(string appName)
        {
            string androidStoreAppVersion = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var doc = client.GetAsync("https://play.google.com/store/apps/details?id=com.wikiteksystems.dtool" + "&hl=en_US").Result.Parse();

                    //var doc = client.GetAsync("https://play.google.com/store/apps/details?id=com.wikiteksystems.dtool").Result.Parse();

                    var versionElement = doc.Select("div:containsOwn(Current Version)");
                    androidStoreAppVersion = versionElement.Text;
                    Supremes.Nodes.Element headElement = versionElement[0];
                    Supremes.Nodes.Elements siblingsOfHead = headElement.SiblingElements;
                    Supremes.Nodes.Element contentElement = siblingsOfHead.First;
                    Supremes.Nodes.Elements childrenOfContentElement = contentElement.Children;
                    Supremes.Nodes.Element childOfContentElement = childrenOfContentElement.First;
                    Supremes.Nodes.Elements childrenOfChildren = childOfContentElement.Children;
                    Supremes.Nodes.Element childOfChild = childrenOfChildren.First;

                    androidStoreAppVersion = childOfChild.Text;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return androidStoreAppVersion;
            //////string androidStoreAppVersion = null;
            //////if (string.IsNullOrWhiteSpace(appName))
            //////{
            //////    throw new ArgumentNullException(nameof(appName));
            //////}

            //////var version = string.Empty;
            ////////var url = $"https://play.google.com/store/apps/details?id={appName}&hl=en_US";
            //////var url = $"https://play.google.com/store/apps/details?id=com.wikiteksystems.dtool&hl=en_US";
            //////using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            //////{
            //////    using (var handler = new HttpClientHandler())
            //////    {
            //////        using (var client = new HttpClient(handler))
            //////        {
            //////            using (var responseMsg = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
            //////            {
            //////                if (!responseMsg.IsSuccessStatusCode)
            //////                {
            //////                    //throw new LatestVersionException($"Error connecting to the Play Store. Url={url}.");
            //////                }

            //////                try
            //////                {
            //////                    var content = responseMsg.Content == null ? null : await responseMsg.Content.ReadAsStringAsync();

            //////                    var versionMatch = Regex.Match(content, "<div[^>]*>Current Version</div><span[^>]*><div><span[^>]*>(.*?)<").Groups[1];

            //////                    if (versionMatch.Success)
            //////                    {
            //////                        version = versionMatch.Value.Trim();
            //////                    }
            //////                }
            //////                catch (Exception e)
            //////                {
            //////                    // throw new LatestVersionException($"Error parsing content from the Play Store. Url={url}.", e);
            //////                }
            //////            }
            //////        }
            //////    }
            //////}

            //////return version;
        }

        /// <inheritdoc />
        public Task OpenAppInStore()
        {
            return OpenAppInStore(_packageName);
        }

        /// <inheritdoc />
        public Task OpenAppInStore(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentNullException(nameof(appName));
            }

            try
            {
                var intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse($"market://details?id={appName}"));
                intent.SetPackage("com.android.vending");
                intent.SetFlags(ActivityFlags.NewTask);
                global::Android.App.Application.Context.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                var intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse($"https://play.google.com/store/apps/details?id={appName}"));
                global::Android.App.Application.Context.StartActivity(intent);
            }

            return Task.FromResult(true);
        }
    }
}