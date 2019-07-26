using EmbAppViewer.Core;
using EmbAppViewer.InterComm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Windows;

namespace EmbAppViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ICommandLineReceiver
    {
        private IList<string> _commandLineArgs;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Win32.AttachConsole(-1);

            try {
                var cliArgs = CliArguments.Parse(e.Args);
                _commandLineArgs = e.Args;
                if (!String.IsNullOrEmpty(cliArgs.Instance))
                {
                    if (CommandLineInterComm<App>.TrySendCommandLineArgs(ConfigurationManager.AppSettings.Get("ApplicationIdentifier"), cliArgs.Instance, e.Args))
                    {
                        Shutdown();
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Shutdown(-1);
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            CommandLineInterComm<App>.Cleanup();
            Win32.FreeConsole();
        }

        private void ProcessCommandLineArgs(IList<string> args)
        {
            var main = MainWindow as Presentation.MainWindow;
            if (main != null)
            {
                var cliArgs = CliArguments.Parse(args);
                if (String.IsNullOrEmpty(cliArgs.Cmd))
                {
                    return;
                }

                var item = new Item(cliArgs);
                var appInstance = new ApplicationInstance(item);
                var startSuccessfull = appInstance.Start();
                if (startSuccessfull)
                {
                    main.EmbeddAppInTab(appInstance);
                }
            }
        }

        public bool ReceiveCommandLineArgs(IList<string> args)
        {
            ProcessCommandLineArgs(args);

            return true;
        }

        private void Application_Activated(object sender, EventArgs e)
        {
            if (_commandLineArgs != null)
            {
                ProcessCommandLineArgs(_commandLineArgs);
                _commandLineArgs = null;
            }
        }
    }
}
