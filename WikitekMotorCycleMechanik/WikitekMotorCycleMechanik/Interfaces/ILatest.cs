using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikitekMotorCycleMechanik.Interfaces
{
    public interface ILatest
    {
        //
        // Summary:
        //     Gets the version number of the current app's installed version.
        string InstalledVersionNumber { get; }

        //
        // Summary:
        //     Gets the version number of the current app's latest version available in the
        //     public store.
        //
        // Returns:
        //     The current app's latest version number.
        Task<string> GetLatestVersionNumber();
        //
        // Summary:
        //     Gets the version number of an app's latest version available in the public store.
        //
        // Parameters:
        //   appName:
        //     Name of the app to get.
        //
        // Returns:
        //     The specified app's latest version number
        Task<string> GetLatestVersionNumber(string appName);
        //
        // Summary:
        //     Checks if the current app is the latest version available in the public store.
        //
        // Returns:
        //     True if the current app is the latest version available, false otherwise.
        Task<bool> IsUsingLatestVersion();
        //
        // Summary:
        //     Opens the current app in the public store.
        Task OpenAppInStore();
        //
        // Summary:
        //     Opens an app in the public store.
        //
        // Parameters:
        //   appName:
        //     Name of the app to open.
        Task OpenAppInStore(string appName);
    }
}
