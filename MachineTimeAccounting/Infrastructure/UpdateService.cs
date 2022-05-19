using System.Deployment.Application;
using System.Windows;

namespace MachineTimeAccounting
{
    static class UpdateService
    {
        public static string GetVersion()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return null;

            return ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4);
        }

        public static bool IsUpdateRequired()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return false;

            return ApplicationDeployment.CurrentDeployment.CheckForDetailedUpdate().UpdateAvailable;
        }

        public static void Update()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return;

            ApplicationDeployment.CurrentDeployment.Update();

            Application.Current.Shutdown();
            System.Windows.Forms.Application.Restart();
        }
    }
}
