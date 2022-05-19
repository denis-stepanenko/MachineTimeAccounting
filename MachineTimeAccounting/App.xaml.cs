using System.Windows;
using System.Windows.Threading;

namespace MachineTimeAccounting
{
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
$@"Ой!
{e.Exception.Message}",
"Необработанное исключение");
        }
    }
}
