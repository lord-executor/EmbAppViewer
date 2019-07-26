using EmbAppViewer.InterComm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

namespace EmbAppViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ICommandLineReceiver
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (CommandLineInterComm<App>.TrySendCommandLineArgs(ConfigurationManager.AppSettings.Get("ApplicationIdentifier"), "Test", Environment.GetCommandLineArgs()))
            {
                Shutdown();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            CommandLineInterComm<App>.Cleanup();
        }

        public bool ReceiveCommandLineArgs(IList<string> args)
        {
            // handle command line arguments of second instance
            // ...
            return true;
        }
    }
}
