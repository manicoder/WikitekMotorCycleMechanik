

using Android.Content.PM;
using WikitekMotorCycleMechanik.Droid.Dependencies;
using WikitekMotorCycleMechanik.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(VersionAndBuildNumber))]
namespace WikitekMotorCycleMechanik.Droid.Dependencies
{
    class VersionAndBuildNumber : IVersionAndBuildNumber
    {
        PackageInfo _appInfo;
        public VersionAndBuildNumber()
        {
            var context = Android.App.Application.Context;
            _appInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
        }
        public string GetVersionNumber()
        {
            return _appInfo.VersionName;
        }
        public string GetBuildNumber()
        {
            return _appInfo.VersionCode.ToString();
        }
    }
}