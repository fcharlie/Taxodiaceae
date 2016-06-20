using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Taxodiaceae
{
    class App
    {
        static int Main(string[] args)
        {
            var filename = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\bin\\Taxodiaceae.exe";
            //MessageBox.Show(filename);
            if (!System.IO.File.Exists(filename))
            {
                MessageBox.Show(filename, "Not Found Taxodiaceae", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            Process ps = new Process();
            ps.StartInfo.FileName = filename;
            try
            {
                ps.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Start Taxodiaceae Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return 0;
        }
    }
}
