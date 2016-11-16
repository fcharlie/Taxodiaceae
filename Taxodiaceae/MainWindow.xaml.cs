using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Taxodiaceae
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        static string ErrorMessage { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            var args=Environment.GetCommandLineArgs();
            if(args.Length>=2 && System.IO.File.Exists(args[1])){
                PackfileUri.Text = args[1];
            }
            this.ArcsLoadingIndicator.IsActive = false;
        }
        static Task<int> TaxodiaceaeTaskInternal(string pack, string folder)
        {
            return Task.Run(() =>
            {
                Process msiexec = new Process();
                msiexec.StartInfo.FileName = "msiexec";
                msiexec.StartInfo.Arguments = String.Format("/a \"{0}\" /qn TARGETDIR=\"{1}\"", pack, folder);
                try {
                    msiexec.Start();
                }
                catch (Exception e)
                {
                    ErrorMessage = e.Message;
                    //Log.Fatal("Start msiexec failed: {0}", e.Message);
                    return -1;
                }
                msiexec.WaitForExit();
                return msiexec.ExitCode;
            });
        }
        static async Task<int> TaxodiaceaeTask(string pack, string folder)
        {
            int ret = await TaxodiaceaeTaskInternal(pack, folder);
            return ret;
        }

        private void ViewPackfileUri(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Get Installer Package";
            dlg.DefaultExt = ".msi";
            dlg.Filter = "Microsoft Installer Package (*.msi;*.msp)|*.msi;*.msp";
            var result = dlg.ShowDialog();
            if (result == true)
            {
                PackfileUri.Text = dlg.FileName;
            }
        }

        private void SelectFolderUri(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
            dlg.IsFolderPicker = true;
            dlg.Title = "Select Unpack Folder:";
            var result = dlg.ShowDialog();
            if (result == Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Ok)
            {
                FolderUri.Text = dlg.FileName;
            }
        }
        private async void TaxodiaceaeRunTaskInvalidArgument()
        {
            await this.ShowMessageAsync("A Param is empty", "Please input your msi pack name and unpack folder !");
        }
        private async void TaxodiaceaeRunTaskResultError(int error)
        {
            string msg =null;
            if (error == -1 && ErrorMessage.Length > 0)
            {
                msg = String.Format("Start msiexec error: {0}", ErrorMessage);
            }
            else
            {
               msg  = String.Format("msiexec ExitCode: {0}", error);
            }
            await this.ShowMessageAsync("Sorry, Unpack failed ", msg);
        }
        private async void TaxodiaceaeRunTaskSuccessed(string path)
        {
            await this.ShowMessageAsync("Unpack success", "Goto to your folder look files");
        }
        private void TaxodiaceaeRunTask(object sender, RoutedEventArgs e)
        {
            //TaxodiaceaeRunTaskResultError(0);
            if (PackfileUri.Text.Length == 0 || FolderUri.Text.Length == 0)
            {
                TaxodiaceaeRunTaskInvalidArgument();
                return;
            }
            this.ArcsLoadingIndicator.IsActive = true;
            RunTaskButton.IsEnabled = false;
            var result = TaxodiaceaeTask(PackfileUri.Text, FolderUri.Text).GetAwaiter();
            result.OnCompleted(() =>
            {
                int ret = result.GetResult();
                if (ret==0)
                {
                    TaxodiaceaeRunTaskSuccessed(FolderUri.Text);                
                }
                else
                {
                    TaxodiaceaeRunTaskResultError(ret);
                }
                this.ArcsLoadingIndicator.IsActive = false;
                RunTaskButton.IsEnabled = true;
            });
        }

        private void PackfileUriPreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void PackfileUriPreviewDrop(object sender, DragEventArgs e)
        {
            PackfileUri.Text = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        }
    }
}
